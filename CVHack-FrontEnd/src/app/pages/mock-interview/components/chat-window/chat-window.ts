import { Component, Input, ElementRef, ViewChild, AfterViewChecked } from '@angular/core';

export type MessageType = 'ai-question' | 'user-answer' | 'ai-feedback';

export interface ChatMessage {
  type: MessageType;
  text: string;
}

@Component({
  selector: 'app-chat-window',
  imports: [],
  templateUrl: './chat-window.html',
  styleUrl: './chat-window.css',
})
export class ChatWindow implements AfterViewChecked {
  @Input() messages: ChatMessage[] = [];
  @ViewChild('scrollArea') scrollArea!: ElementRef<HTMLDivElement>;

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  scrollToBottom() {
    if (this.scrollArea) {
      this.scrollArea.nativeElement.scrollTop = this.scrollArea.nativeElement.scrollHeight;
    }
  }
}
