import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProjectsService, ProjectItem } from '../../../services/projects';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './projects.html',
  styleUrl: './projects.css'
})
export class Projects implements OnInit {
  projectsList: ProjectItem[] = [];
  isLoading = true;
  showForm = false;
  isSaving = false;
  errors: any = {};

  formData: ProjectItem = {
    title: '',
    description: null,
    githubUrl: null
  };

  editingId: string | null = null;

  constructor(
    private projectsService: ProjectsService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadProjects();
  }

  loadProjects() {
    this.isLoading = true;
    this.projectsService.getProjects().subscribe({
      next: (res: any) => {
        console.log('Projects:', res);
        this.projectsList = res?.data || res || [];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error:', err);
        this.projectsList = [];
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  openAddForm() {
    this.editingId = null;
    this.errors = {};
    this.formData = { title: '', description: null, githubUrl: null };
    this.showForm = true;
  }

  openEditForm(item: ProjectItem) {
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

  if (!this.formData.title?.trim()) {
    this.errors.title = 'Project title is required';
  }

  if (this.formData.description && this.formData.description.length > 2000) {
  this.errors.description = `Description is too long (${this.formData.description.length}/2000 characters)`;
}

  if (this.formData.githubUrl?.trim()) {
    const urlPattern = /^https?:\/\/.+/;
    if (!urlPattern.test(this.formData.githubUrl.trim())) {
      this.errors.githubUrl = 'Please enter a valid URL (e.g. https://github.com/...)';
    }
  }

  return Object.keys(this.errors).length === 0;
}

  onSave() {
    if (!this.validateForm()) return;
    this.isSaving = true;

    if (this.editingId) {
      this.projectsService.updateProject(this.editingId, this.formData).subscribe({
        next: () => {
          this.loadProjects();
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
      this.projectsService.addProject(this.formData).subscribe({
        next: () => {
          this.loadProjects();
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

  deleteProject(id: string) {
    this.projectsService.deleteProject(id).subscribe({
      next: () => {
        this.projectsList = this.projectsList.filter(p => p.id !== id);
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Error:', err)
    });
  }
}