import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface EducationItem {
  id?: string;
  university?: string;
  degree?: string;
  startYear?: number | null;
  endYear?: number | null;
  grade?: string | null;
}

@Injectable({
  providedIn: 'root'
})
export class EducationService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  }

  getEducation(): Observable<any> {
  return this.http.get<any>(`${this.apiUrl}/profile/education`);
}

addEducation(data: EducationItem): Observable<any> {
  return this.http.post<any>(`${this.apiUrl}/profile/education`, data);
}

updateEducation(id: string, data: EducationItem): Observable<any> {
  return this.http.put<any>(`${this.apiUrl}/profile/education/${id}`, data);
}

deleteEducation(id: string): Observable<any> {
  return this.http.delete<any>(`${this.apiUrl}/profile/education/${id}`);
}
  }