import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileHeader } from '../profile-header/profile-header';
import { ProfileTabs } from '../profile-tabs/profile-tabs';
import { PersonalInfo } from '../personal-info/personal-info';
import { Skills } from '../skills/skills';
import { Experience } from '../experience/experience';
import { Education } from '../education/education';
import { Certifications } from '../certifications/certifications';
import { Projects } from '../projects/projects';
import { Navbar } from '../../../components/navbar/navbar';
import { ProfileService, ProfileData } from '../../../services/profile';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule, Navbar,
    ProfileHeader, ProfileTabs, PersonalInfo,
    Skills, Experience, Education, Certifications, Projects
  ],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile implements OnInit {

  fullName = '';
  jobTitle = '';
  location = '';
  email = '';
  phone = '';
  initials = '';
  rawProfileData: any = null;

  isLoading = true;
  errorMessage = '';
  activeTab = 'personal-info';

  constructor(
  private profileService: ProfileService,
  private authService: AuthService,
  private cdr: ChangeDetectorRef
) {}

  ngOnInit() {
  const firstName = localStorage.getItem('firstName') ?? '';
  const lastName = localStorage.getItem('lastName') ?? '';
  const email = localStorage.getItem('email') ?? '';

  console.log('firstName:', firstName);
  console.log('lastName:', lastName);

  this.fullName = [firstName, lastName].filter(x => x).join(' ') || 'User';
  this.initials = this.getInitials(this.fullName);
  this.email = email;

  this.loadProfile();
}


loadProfile() {
  this.isLoading = true;

  this.profileService.getProfile().subscribe({
    next: (res: any) => {
      try {
        const data = res.data || res;
        this.rawProfileData = data;
        this.jobTitle = data?.jobTitle || data?.headline || '';
        this.location = [data?.city, data?.country].filter(x => x).join(', ');
        this.phone = data?.phoneNumber || '';
      } catch(e) {
        console.error('Error parsing data:', e);
      }
      this.isLoading = false;
      this.cdr.detectChanges(); 
    },
    error: (err) => {
      console.log('ERROR:', err);
      this.isLoading = false;
      this.cdr.detectChanges();
    }
  });
}

  getInitials(name: string): string {
    if (!name || typeof name !== 'string') return '??';
    const parts = name.trim().split(' ').filter(p => p.length > 0);
    if (parts.length === 0) return '??';
    return parts.slice(0, 2).map(n => n[0]).join('').toUpperCase();
  }

  onTabChange(tabId: string) {
    this.activeTab = tabId;
  }
}
