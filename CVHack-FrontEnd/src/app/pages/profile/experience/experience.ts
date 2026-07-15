import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ExperienceService, ExperienceItem } from '../../../services/experience';

@Component({
  selector: 'app-experience',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './experience.html',
  styleUrl: './experience.css'
})
export class Experience implements OnInit {
  experienceList: ExperienceItem[] = [];
  isLoading = true;
  showForm = false;
  isSaving = false;
  errors: any = {};

  formData: ExperienceItem = {
    companyName: '',
    jobTitle: '',
    startDate: '',
    endDate: null,
    description: ''
  };

  editingId: string | null = null;

  constructor(
    private experienceService: ExperienceService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadExperience();
  }

  loadExperience() {
    this.isLoading = true;
    this.experienceService.getExperience().subscribe({
      next: (res: any) => {
        this.experienceList = res?.data || res || [];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.experienceList = [];
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  openAddForm() {
    this.editingId = null;
    this.errors = {};
    this.formData = {
      companyName: '',
      jobTitle: '',
      startDate: '',
      endDate: null,
      description: ''
    };
    this.showForm = true;
  }

  openEditForm(item: ExperienceItem) {
  this.editingId = item.id || null;
  this.errors = {};
  this.formData = {
    ...item,
    startDate: item.startDate ? new Date(item.startDate).toISOString().split('T')[0] : '',
    endDate: item.endDate ? new Date(item.endDate).toISOString().split('T')[0] : null,
  };
  this.showForm = true;
}

  onCancel() {
    this.showForm = false;
    this.editingId = null;
    this.errors = {};
  }

  validateForm(): boolean {
    this.errors = {};

    if (!this.formData.jobTitle?.trim()) {
      this.errors.jobTitle = 'Job title is required';
    }

    if (!this.formData.companyName?.trim()) {
      this.errors.companyName = 'Company name is required';
    }

    if (!this.formData.startDate) {
      this.errors.startDate = 'Start date is required';
    } else {
      const date = new Date(this.formData.startDate);
      if (isNaN(date.getTime())) {
        this.errors.startDate = 'Please enter a valid date';
      }
    }

    if (this.formData.endDate) {
      const date = new Date(this.formData.endDate);
      if (isNaN(date.getTime())) {
        this.errors.endDate = 'Please enter a valid date';
      }
    }

    return Object.keys(this.errors).length === 0;
  }

  onSave() {
    if (!this.validateForm()) return;
    this.isSaving = true;

    const payload = {
      companyName: this.formData.companyName,
      jobTitle: this.formData.jobTitle,
      startDate: this.formData.startDate || null,
      endDate: this.formData.endDate || null,
    };

    if (this.editingId) {
      this.experienceService.updateExperience(this.editingId, payload as any).subscribe({
        next: () => {
          this.loadExperience();
          this.showForm = false;
          this.isSaving = false;
          this.errors = {};
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Error:', err);
          this.isSaving = false;
        }
      });
    } else {
      this.experienceService.addExperience(payload as any).subscribe({
        next: () => {
          this.loadExperience();
          this.showForm = false;
          this.isSaving = false;
          this.errors = {};
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Error:', err);
          this.isSaving = false;
        }
      });
    }
  }

  deleteExperience(id: string) {
    this.experienceService.deleteExperience(id).subscribe({
      next: () => {
        this.experienceList = this.experienceList.filter(e => e.id !== id);
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Error:', err)
    });
  }
}