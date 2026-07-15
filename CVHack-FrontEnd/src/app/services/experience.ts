import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface ExperienceItem {
  id?: string;
  companyName: string;
  jobTitle: string;
  startDate: string;
  endDate?: string | null;
  description?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ExperienceService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  }

  getExperience(): Observable<ExperienceItem[]> {
    return this.http.get<ExperienceItem[]>(`${this.apiUrl}/profile/experience`, {
      
    });
  }

  addExperience(data: ExperienceItem): Observable<ExperienceItem> {
    return this.http.post<ExperienceItem>(`${this.apiUrl}/profile/experience`, data, {
      
    });
  }

  updateExperience(id: string, data: ExperienceItem): Observable<ExperienceItem> {
    return this.http.put<ExperienceItem>(`${this.apiUrl}/profile/experience/${id}`, data, {
      
    });
  }

  deleteExperience(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/profile/experience/${id}`, {
      
    });
  }
}