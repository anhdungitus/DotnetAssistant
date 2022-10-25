import {AfterViewInit, Component, OnInit} from '@angular/core';
import {PageEvent} from "@angular/material/paginator";
import {Question, QuestionService} from "./question.service";

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements AfterViewInit {
  pageEvent?: PageEvent;
  pageSizeOptions: number[] = [5, 10, 25, 100];

  constructor(
    private questionService: QuestionService
  ) { }

  title = 'MatTable';
  displayedColumns: string[] = ['id', 'text', 'createdOn'];
  dataSource: Question[] = [];

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput.split(',').map(str => +str);
    }
  }

  public updateData(event?:PageEvent){
    this.dataSource = this.dataSource.slice(1, 2);
    return event;
  }

  ngAfterViewInit(): void {
    this.questionService.getQuestion().subscribe(r => this.dataSource = r);
  }
}


