import { Component, OnInit, OnDestroy, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { ChatService } from '../../providers/chat.service';
import { Subscription } from 'rxjs';
import { Message } from '../../models/message.model';

@Component({
  selector: 'app-message-list',
  templateUrl: './message-list.component.html',
  styleUrls: ['./message-list.component.scss']
})
export class MessageListComponent implements OnInit, OnDestroy {
  messages: Message[] = [];
  messageSubscription: Subscription;
  constructor(private chatService: ChatService) {
    // EMPTY
  }

  ngOnInit() {
    this.messageSubscription = this.chatService.receivedMessages$.subscribe(msg => {
      this.messages = [...this.messages, msg];
    });
  }

  ngOnDestroy() {
    this.messageSubscription.unsubscribe();
  }
}
