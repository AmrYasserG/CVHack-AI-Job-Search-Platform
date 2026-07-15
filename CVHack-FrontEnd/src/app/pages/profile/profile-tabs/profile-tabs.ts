import { Component, Output, EventEmitter, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile-tabs',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile-tabs.html',
  styleUrl: './profile-tabs.css'
})
export class ProfileTabs {
  @Input() activeTab: string = 'personal-info';
  @Output() tabChange = new EventEmitter<string>();

  tabs = [
    { id: 'personal-info', label: 'Personal Info' },
    { id: 'skills', label: 'Skills' },
    { id: 'experience', label: 'Experience' },
    { id: 'education', label: 'Education' },
    { id: 'certifications', label: 'Certifications' },
    { id: 'projects', label: 'Projects' },
  ];

  selectTab(tabId: string) {
    this.activeTab = tabId;
    this.tabChange.emit(tabId);
  }
}