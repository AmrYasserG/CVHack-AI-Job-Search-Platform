import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-chat-input',
  imports: [FormsModule],
  templateUrl: './chat-input.html',
  styleUrl: './chat-input.css',
})
export class ChatInput {
  @Output() sendMessage = new EventEmitter<string>();

  text = '';
  isRecording = false;

  send() {
    const trimmed = this.text.trim();
    if (!trimmed) return;
    this.sendMessage.emit(trimmed);
    this.text = '';
  }

  onKeyDown(event: KeyboardEvent) {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      this.send();
    }
  }

  toggleRecording() {
    this.isRecording = !this.isRecording;
  }
}
