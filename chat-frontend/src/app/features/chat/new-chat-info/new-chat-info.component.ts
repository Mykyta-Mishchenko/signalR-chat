import { Component, inject, input } from '@angular/core';
import { ChatParticipant } from '../models/chat-participant.model';
import { ChatService } from '../services/chat.service';
import { NewChatDto } from '../dto/new-chat.dto';
import { AuthService } from '../../../core/services/auth/auth.service';
import { ChatType } from '../models/chat-type.enum';

@Component({
  selector: 'app-new-chat-info',
  standalone: true,
  imports: [],
  templateUrl: './new-chat-info.component.html',
  styleUrl: './new-chat-info.component.css'
})
export class NewChatInfoComponent {
  private chatService = inject(ChatService);
  private authService = inject(AuthService);

  user = input.required<ChatParticipant>();
  isChatFriend = input.required<boolean>();

  onCreateChat(userId: number) {
    const newChat: NewChatDto = {
      name: "",
      creatorId: this.authService.User()!.userId,
      chatType: ChatType.Personal,
      participants: [
        { id: userId, name: "" },
        { id:this.authService.User()!.userId, name: ""}
      ]
    };
    this.chatService.createNewChat(newChat);
  }
}
