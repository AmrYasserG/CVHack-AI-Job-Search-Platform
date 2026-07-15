import { Component, Input, OnChanges, SimpleChanges, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProfileService } from '../../../services/profile';

@Component({
  selector: 'app-personal-info',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './personal-info.html',
  styleUrl: './personal-info.css'
})
export class PersonalInfo implements OnChanges {
  @Input() fullName: string = '';
  @Input() email: string = '';
  @Input() profileData: any = null;

  phone = '';
  location = '';
  headline = '';
  country = '';
  summary = '';
  linkedInUrl = '';
  gitHubUrl = '';
  portfolioUrl = '';

  isEditing = false;
  isSaving = false;
  successMessage = '';
  errorMessage = '';
  errors: any = {};

  editData = {
  phone: '',
  city: '',
  country: '',
  headline: '',
  summary: '',
  linkedInUrl: '',
  gitHubUrl: '',
  portfolioUrl: ''
};

  constructor(
    private profileService: ProfileService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes['profileData'] && this.profileData) {
      this.phone = this.profileData.phoneNumber || '';
      this.location = this.profileData.city || '';
      this.country = this.profileData.country || '';
      this.headline = this.profileData.headline || '';
      this.summary = this.profileData.summary || '';
      this.linkedInUrl = this.profileData.linkedInUrl || '';
      this.gitHubUrl = this.profileData.gitHubUrl || '';
      this.portfolioUrl = this.profileData.portfolioUrl || '';
      this.cdr.detectChanges();
    }
  }

  validateForm(): boolean {
    this.errors = {};

    if (this.editData.phone && !/^[+\d\s\-()]+$/.test(this.editData.phone)) {
      this.errors.phone = 'Phone must contain numbers only';
    }

    if (this.editData.linkedInUrl && !this.editData.linkedInUrl.startsWith('https://')) {
      this.errors.linkedInUrl = 'LinkedIn URL must start with https://';
    }

    if (this.editData.gitHubUrl && !this.editData.gitHubUrl.startsWith('https://')) {
      this.errors.gitHubUrl = 'GitHub URL must start with https://';
    }

    if (this.editData.portfolioUrl && !this.editData.portfolioUrl.startsWith('https://')) {
      this.errors.portfolioUrl = 'Portfolio URL must start with https://';
    }

    return Object.keys(this.errors).length === 0;
  }

  onEdit() {
    this.errors = {};
    this.editData = {
      phone: this.phone,
      country: this.country,
      city: this.location,
      headline: this.headline,
      summary: this.summary,
      linkedInUrl: this.linkedInUrl,
      gitHubUrl: this.gitHubUrl,
      portfolioUrl: this.portfolioUrl
    };
    this.isEditing = true;
    this.successMessage = '';
    this.errorMessage = '';
  }

  onCancel() {
    this.isEditing = false;
    this.errors = {};
  }

  onSave() {
    if (!this.validateForm()) return;
    this.isSaving = true;
    this.successMessage = '';
    this.errorMessage = '';

    this.profileService.updateProfile({
      phoneNumber: this.editData.phone,
      city: this.editData.city,
      country: this.editData.country,
      headline: this.editData.headline,
      summary: this.editData.summary,
      linkedInUrl: this.editData.linkedInUrl,
      gitHubUrl: this.editData.gitHubUrl,
      portfolioUrl: this.editData.portfolioUrl
    } as any).subscribe({
      next: (res: any) => {
        const data = res?.data;
        this.phone = data?.phoneNumber || this.editData.phone || '';
        this.location = data?.city || this.editData.city || '';
        this.country = data?.country || this.editData.country || '';
        this.headline = data?.headline || this.editData.headline || '';
        this.summary = data?.summary || this.editData.summary || '';
        this.linkedInUrl = data?.linkedInUrl || this.editData.linkedInUrl || '';
        this.gitHubUrl = data?.gitHubUrl || this.editData.gitHubUrl || '';
        this.portfolioUrl = data?.portfolioUrl || this.editData.portfolioUrl || '';

        this.successMessage = 'Profile updated successfully!';
        this.isEditing = false;
        this.isSaving = false;
        this.errors = {};
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error:', err);
        this.errorMessage = 'Failed to update profile. Please try again.';
        this.isSaving = false;
        this.cdr.detectChanges();
      }
    });
  }
}