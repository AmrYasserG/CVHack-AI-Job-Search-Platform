import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SkillsService } from '../../../services/skills';

interface Skill {
  skillId?: number;
  skillName?: string;
  id?: string;
  name?: string;
  [key: string]: any;
}

@Component({
  selector: 'app-skills',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './skills.html',
  styleUrl: './skills.css'
})
export class Skills implements OnInit {
  skills: Skill[] = [];
  searchResults: Skill[] = [];
  searchQuery = '';
  isLoading = true;
  isSearching = false;
  showDropdown = false;

  constructor(
    private skillsService: SkillsService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadSkills();
  }

  loadSkills() {
    this.isLoading = true;
    this.skillsService.getProfileSkills().subscribe({
      next: (res: any) => {
        console.log('Skills:', res);
        this.skills = res?.data || res || [];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading skills:', err);
        this.skills = [];
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  onSearch() {
    if (!this.searchQuery.trim()) {
      this.searchResults = [];
      this.showDropdown = false;
      return;
    }

    this.isSearching = true;
    this.skillsService.searchSkills(this.searchQuery).subscribe({
      next: (res: any) => {
        console.log('Search results:', res);
        const allResults = res?.data || res || [];
        
        this.searchResults = allResults.filter(
          (s: Skill) => !this.skills.find(ps => ps.id === s.id)
        );
        this.showDropdown = this.searchResults.length > 0;
        this.isSearching = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error searching skills:', err);
        this.isSearching = false;
        this.cdr.detectChanges();
      }
    });
  }

  addSkill(skill: Skill) {
  const id = skill.skillId || skill.id;
  this.skillsService.addSkill(String(id)).subscribe({
    next: (res: any) => {
      this.skills.push(skill);
      this.searchQuery = '';
      this.searchResults = [];
      this.showDropdown = false;
      this.cdr.detectChanges();
    },
    error: (err) => console.error('Error adding skill:', err)
  });
}

removeSkill(skill: Skill) {
  const id = skill.skillId || skill.id;
  this.skillsService.removeSkill(String(id)).subscribe({
    next: () => {
      this.skills = this.skills.filter(
        s => (s.skillId || s.id) !== (skill.skillId || skill.id)
      );
      this.cdr.detectChanges();
    },
    error: (err) => console.error('Error removing skill:', err)
  });
}
  

  hideDropdown() {
    setTimeout(() => {
      this.showDropdown = false;
      this.cdr.detectChanges();
    }, 200);
  }
}