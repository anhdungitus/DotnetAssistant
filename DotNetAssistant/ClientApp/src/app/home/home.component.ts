import { Component, OnInit } from '@angular/core';
import {Observable, Subject} from "rxjs";
import {CommandService} from "../command.service";
import {debounceTime, distinctUntilChanged, switchMap} from "rxjs/operators";
import { AuthGuard } from '../auth.guard';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
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
