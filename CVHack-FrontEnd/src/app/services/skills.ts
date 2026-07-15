import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';


export interface Skill {
  id: string;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class SkillsService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  }

  searchSkills(query: string): Observable<any> {
  return this.http.get<any>(`${this.apiUrl}/Skills?search=${query}`);
}

getProfileSkills(): Observable<any> {
  return this.http.get<any>(`${this.apiUrl}/profile/skills`);
}

addSkill(skillId: string): Observable<any> {
  return this.http.post(`${this.apiUrl}/profile/skills/${skillId}`, {});
}

removeSkill(skillId: string): Observable<any> {
  return this.http.delete(`${this.apiUrl}/profile/skills/${skillId}`);

  }
}