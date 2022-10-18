import { Component } from '@angular/core';
import { Observable, Subject } from 'rxjs';

import {
  debounceTime, distinctUntilChanged, switchMap
} from 'rxjs/operators';

import { CommandService } from './command.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  result! : Observable<string[]>;
  title = '.NET developer assistant';

  private searchTerms = new Subject<string>();

  constructor(private commandService: CommandService) {}

  search(value: string) {
    this.searchTerms.next(value);
  }

  ngOnInit(): void {
    this.result = this.searchTerms.pipe(
      // wait 300ms after each keystroke before considering the term
      debounceTime(300),

      // ignore new term if same as previous term
      distinctUntilChanged(),

      // switch to new search observable each time the term changes
      switchMap((term: string) => this.commandService.searchService(term)),
    );
  }
}
