import {AfterViewInit, Component, OnInit} from '@angular/core';
import {PageEvent} from "@angular/material/paginator";
import {Question, QuestionService} from "./question.service";
import {MatDialog} from "@angular/material/dialog";
import {EditQuestionComponent} from "./edit-question/edit-question.component";
import {AddQuestionComponent} from "./add-question/add-question.component";
import {Sort} from "@angular/material/sort";

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements AfterViewInit {
  pageEvent?: PageEvent;
  pageSizeOptions: number[] = [5, 10, 25, 100];

  constructor(
    private questionService: QuestionService,
    public dialog: MatDialog
  ) { }

  title = 'MatTable';
  displayedColumns: string[] = ['id', 'text', 'createdOn', 'Action'];
  dataSource: Question[] = [];

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput.split(',').map(str => +str);
    }
  }

  public updateData(event:PageEvent){
    this.questionService.getQuestion(event.pageIndex, event?.pageSize).subscribe(r => this.dataSource = r);
    return event;
  }

  ngAfterViewInit(): void {
    this.questionService.getQuestion(0, 10).subscribe(r => this.dataSource = r);
  }

  edit(question: Question) {
    const dialogRef = this.dialog.open(EditQuestionComponent, {
      width: '250px',
      data: question
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  delete(id: number) {
    this.questionService.deleteQuestion(id).subscribe();
  }

  addData() {
    const dialogRef = this.dialog.open(AddQuestionComponent, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  applyFilter($event: KeyboardEvent) {
    console.log($event);
  }

  sortData($event: Sort) {
    console.log($event);
  }
}


