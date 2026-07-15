import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';


export interface ProjectItem {
  id?: string;
  title: string;
  description?: string | null;
  githubUrl?: string | null;
}

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  }

  getProjects(): Observable<any> {
  return this.http.get<any>(`${this.apiUrl}/profile/projects`);
}

addProject(data: ProjectItem): Observable<any> {
  return this.http.post<any>(`${this.apiUrl}/profile/projects`, data);
}

updateProject(id: string, data: ProjectItem): Observable<any> {
  return this.http.put<any>(`${this.apiUrl}/profile/projects/${id}`, data);
}

deleteProject(id: string): Observable<any> {
  return this.http.delete<any>(`${this.apiUrl}/profile/projects/${id}`);
}
}