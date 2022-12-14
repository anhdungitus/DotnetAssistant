import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CommandService {
  private apiCommandUrl = 'api/Command';

  constructor(
    private http: HttpClient,
  ) { }

  searchService(term: string) {
    let authToken = localStorage.getItem('auth_token');
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        Authorization: `Bearer ${authToken}`
      })
    }

    if (!term.trim()) {
      return of([]);
    }
    return this.http.get<string[]>(`${this.apiCommandUrl}/${term}`, httpOptions);
  }
}
