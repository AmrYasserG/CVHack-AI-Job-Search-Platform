import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EducationService, EducationItem } from '../../../services/education';

@Component({
  selector: 'app-education',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './education.html',
  styleUrl: './education.css'
})
export class Education implements OnInit {
  educationList: EducationItem[] = [];
  isLoading = true;
  showForm = false;
  isSaving = false;
  errors: any = {};

  formData: EducationItem = {
    university: '',
    degree: '',
    startYear: null,
    endYear: null,
    grade: null
  };

  editingId: string | null = null;

  constructor(
    private educationService: EducationService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadEducation();
  }

  loadEducation() {
    this.isLoading = true;
    this.educationService.getEducation().subscribe({
      next: (res: any) => {
        this.educationList = res?.data || res || [];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.educationList = [];
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  openAddForm() {
    this.editingId = null;
    this.errors = {};
    this.formData = {
      university: '',
      degree: '',
      startYear: null,
      endYear: null,
      grade: null
    };
    this.showForm = true;
  }

  openEditForm(item: EducationItem) {
    this.editingId = item.id || null;
    this.errors = {};
    this.formData = { ...item };
    this.showForm = true;
  }

  onCancel() {
    this.showForm = false;
    this.editingId = null;
    this.errors = {};
  }

  validateForm(): boolean {
    this.errors = {};

    if (!this.formData.degree?.trim()) {
      this.errors.degree = 'Degree is required';
    }

    if (!this.formData.university?.trim()) {
      this.errors.university = 'University is required';
    }

    if (this.formData.startYear) {
      const year = Number(this.formData.startYear);
      if (isNaN(year) || year < 1950 || year > 2100) {
        this.errors.startYear = 'Please enter a valid year (e.g. 2018)';
      }
    }

    if (this.formData.endYear) {
      const year = Number(this.formData.endYear);
      if (isNaN(year) || year < 1950 || year > 2100) {
        this.errors.endYear = 'Please enter a valid year (e.g. 2022)';
      }
    }

    if (this.formData.startYear && this.formData.endYear) {
      if (Number(this.formData.endYear) < Number(this.formData.startYear)) {
        this.errors.endYear = 'End year must be after start year';
      }
    }

    return Object.keys(this.errors).length === 0;
  }

  onSave() {
    if (!this.validateForm()) return;
    this.isSaving = true;

    if (this.editingId) {
      this.educationService.updateEducation(this.editingId, this.formData).subscribe({
        next: () => {
          this.loadEducation();
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
      this.educationService.addEducation(this.formData).subscribe({
        next: () => {
          this.loadEducation();
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

  deleteEducation(id: string) {
    this.educationService.deleteEducation(id).subscribe({
      next: () => {
        this.educationList = this.educationList.filter(e => e.id !== id);
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Error:', err)
    });
  }
}