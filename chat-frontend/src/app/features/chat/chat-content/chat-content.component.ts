import { AfterViewInit, Component, effect, ElementRef, inject, OnDestroy, ViewChild } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { SendMessageDto } from '../dto/send-message.dto';
import { AuthService } from '../../../core/services/auth/auth.service';
import { InViewportModule } from 'ng-in-viewport';
import { ChatMessage } from '../models/chat-message.model';
import { TextSentiment } from '../models/text-sentiment.enum';

@Component({
  selector: 'app-chat-content',
  standalone: true,
  imports: [InViewportModule],
  templateUrl: './chat-content.component.html',
  styleUrl: './chat-content.component.css'
})
export class ChatContentComponent implements AfterViewInit, OnDestroy {
  @ViewChild('chatMessagesContainer') chatMessagesContainer!: ElementRef<HTMLDivElement>;
  
  // Your existing properties...
  private chatService = inject(ChatService);
  private authService = inject(AuthService);
  
  // Add these new properties
  private hasScrolledToUnread = false;
  private isInitialLoad = true;
  private isViewInitialized = false;
  
  currentUserId = this.authService.User()!.userId;
  chat = this.chatService.selectedChat;
  messages = this.chatService.chatMessages;

  constructor() {
    // Add these effects to your constructor
    // Effect to handle auto-scroll when messages change
    effect(() => {
      const currentMessages = this.messages();
      const currentChat = this.chat();
      
      console.log('Messages effect triggered:', {
        messageCount: currentMessages?.length || 0,
        hasChat: !!currentChat,
        hasContainer: !!this.chatMessagesContainer,
        isViewInitialized: this.isViewInitialized,
        isInitialLoad: this.isInitialLoad
      });
      
      // Only proceed if view is initialized and we have messages
      if (this.isViewInitialized && currentChat && currentMessages && currentMessages.length > 0) {
        // Add a delay to ensure DOM is updated with the messages
        setTimeout(() => {
          this.handleAutoScroll();
        }, 100);
      }
    });

    // Effect to handle chat changes
    effect(() => {
      const currentChat = this.chat();
      if (currentChat) {
        this.resetScrollState();
      }
    });
  }

  // Add these lifecycle methods
  ngAfterViewInit() {
    console.log('ngAfterViewInit called');
    this.isViewInitialized = true;
    
    // Now that view is initialized, check if we have messages and need to scroll
    setTimeout(() => {
      console.log('AfterViewInit timeout', {
        hasContainer: !!this.chatMessagesContainer,
        messageCount: this.messages().length
      });
      
      if (this.messages().length > 0) {
        this.handleAutoScroll();
      }
    }, 100);
  }

  // Add this new method to handle initial scroll more reliably
  private initializeScroll() {
    if (!this.chatMessagesContainer) return;
    
    // Try multiple times with increasing delays to catch when messages load
    const tryScroll = (attempt: number = 1) => {
      if (this.messages().length > 0) {
        this.handleAutoScroll();
      } else if (attempt < 5) {
        setTimeout(() => tryScroll(attempt + 1), 200 * attempt);
      }
    };
    
    tryScroll();
  }

  ngOnDestroy() {
    this.resetScrollState();
  }

  // Add all these new methods to your component

  private resetScrollState() {
    this.hasScrolledToUnread = false;
    this.isInitialLoad = true;
    // Don't reset isViewInitialized when changing chats
  }

  private handleAutoScroll() {
    if (!this.chatMessagesContainer || !this.messages().length) {
      console.log('handleAutoScroll: Not ready', {
        hasContainer: !!this.chatMessagesContainer,
        messageCount: this.messages().length
      });
      return;
    }

    console.log('handleAutoScroll: Processing', {
      isInitialLoad: this.isInitialLoad,
      hasUnread: this.hasUnreadMessages(),
      messageCount: this.messages().length
    });

    const hasUnread = this.hasUnreadMessages();
    
    if (this.isInitialLoad) {
      if (hasUnread && !this.hasScrolledToUnread) {
        // On initial load, scroll to first unread message
        console.log('Scrolling to first unread');
        setTimeout(() => this.scrollToFirstUnread(), 100);
        this.hasScrolledToUnread = true;
      } else {
        // No unread messages, scroll to bottom
        console.log('Scrolling to bottom (no unread)');
        setTimeout(() => this.scrollToBottom(), 100);
      }
      this.isInitialLoad = false;
    } else {
      // For new messages, scroll to bottom if user was already at bottom
      if (this.isUserAtBottom()) {
        console.log('Scrolling to bottom (new messages)');
        setTimeout(() => this.scrollToBottom(), 50);
      }
    }
  }

  private isUserAtBottom(): boolean {
    const container = this.chatMessagesContainer.nativeElement;
    const threshold = 100; // pixels from bottom
    return container.scrollTop + container.clientHeight >= container.scrollHeight - threshold;
  }

  scrollToFirstUnread() {
    const firstUnreadMessage = this.getFirstUnreadMessage();
    if (firstUnreadMessage) {
      const messageElement = document.getElementById(`message-${firstUnreadMessage.id}`);
      if (messageElement) {
        messageElement.scrollIntoView({ 
          behavior: 'smooth', 
          block: 'start' 
        });
        this.hasScrolledToUnread = true;
      }
    }
  }

  scrollToBottom() {
    if (this.chatMessagesContainer) {
      const container = this.chatMessagesContainer.nativeElement;
      console.log('Scrolling to bottom', {
        scrollHeight: container.scrollHeight,
        clientHeight: container.clientHeight
      });
      container.scrollTop = container.scrollHeight;
    }
  }

  private getFirstUnreadMessage(): ChatMessage | null {
    return this.messages().find(message => 
      !message.isRead && message.senderId !== this.currentUserId
    ) || null;
  }

  isFirstUnreadMessage(message: ChatMessage): boolean {
    const firstUnread = this.getFirstUnreadMessage();
    return firstUnread?.id === message.id;
  }

  hasUnreadMessages(): boolean {
    return this.messages().some(message => 
      !message.isRead && message.senderId !== this.currentUserId
    );
  }

  getUnreadCount(): number {
    return this.messages().filter(message => 
      !message.isRead && message.senderId !== this.currentUserId
    ).length;
  }

  // Your existing methods remain the same...
  onSendMessage(formMessage: string) {
    if (!formMessage.trim()) return;
    
    console.log(formMessage);
    const chatMessage: SendMessageDto = {
      chatId: this.chat()!.id,
      senderId: this.currentUserId,
      content: formMessage
    };
    this.chatService.sendChatMessage(chatMessage);
  }

  getMessageClass(senderId: number) {
    return senderId == this.currentUserId ? 'sent' : 'received';
  }

  onMessageVisible(event: {visible: boolean}, message: ChatMessage) {
    if (event.visible && !message.isRead && message.senderId != this.currentUserId) {
      this.chatService.setChatMessageAsRead(message.id);
    }
  }

  isMessageMine(message: ChatMessage) {
    return message.senderId == this.currentUserId;
  }

  formatTimestamp(timestamp: string | Date): string {
    const date = typeof timestamp === 'string' ? new Date(timestamp) : timestamp;
    
    return date.toLocaleTimeString('en-US', {
        hour: 'numeric',
        minute: '2-digit',
        hour12: true
    });
  }

  isMessageUnread(message: ChatMessage) {
    return !message.isRead && message.senderId != this.currentUserId;
  }

  getMessageSentiment(message: ChatMessage) {
    switch (message.sentiment) {
      case TextSentiment.Positive:
        return 'bi bi-emoji-smile';
      case TextSentiment.Neutral:
        return 'bi bi-emoji-neutral';
      case TextSentiment.Negative:
        return 'bi bi-emoji-angry';
      default:
        return 'bi bi-question';
    }
  }
}
