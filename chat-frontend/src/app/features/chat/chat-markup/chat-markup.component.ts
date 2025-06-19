import { Component } from '@angular/core';
import { ChatsListComponent } from "../chats-list/chats-list.component";
import { ChatContentComponent } from "../chat-content/chat-content.component";
import { GroupCreateComponent } from "../group-create/group-create.component";

@Component({
  selector: 'app-chat-markup',
  standalone: true,
  imports: [ChatsListComponent, ChatContentComponent, GroupCreateComponent],
  templateUrl: './chat-markup.component.html',
  styleUrl: './chat-markup.component.css'
})
export class ChatMarkupComponent {

}
