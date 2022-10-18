import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CommandService {
  private apiCommandUrl = 'api/Command';

  constructor(
    private http: HttpClient,
  ) { }

  searchService(term: string) {
    if (!term.trim()) {
      return of([]);
    }
    return this.http.get<string[]>(`${this.apiCommandUrl}/${term}`);
  }
}
