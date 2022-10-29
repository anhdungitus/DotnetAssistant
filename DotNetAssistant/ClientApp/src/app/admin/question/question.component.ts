import {AfterViewInit, Component, OnInit} from '@angular/core';
import {PageEvent} from "@angular/material/paginator";
import {Question, QuestionService} from "./question.service";
import {MatDialog} from "@angular/material/dialog";
import {EditQuestionComponent} from "./edit-question/edit-question.component";
import {AddQuestionComponent} from "./add-question/add-question.component";
import {Sort} from "@angular/material/sort";
import {ConfirmationDialogComponent} from "./confirmation-dialog/confirmation-dialog.component";

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements AfterViewInit {
  pageEvent?: PageEvent;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageIndex= 0;
  pageSize= 10;
  sortActive='id';
  sortDirection='asc';
  keyword='';

  constructor(
    private questionService: QuestionService,
    public dialog: MatDialog
  ) { }

  title = 'Table';
  displayedColumns: string[] = ['id', 'text', 'createdOn', 'Action'];
  dataSource: Question[] = [];

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput.split(',').map(str => +str);
    }
  }

  public updateData(event:PageEvent){
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.refreshData();
    return event;
  }

  public refreshData() {
    this.questionService.getQuestion(this.pageIndex, this.pageSize, this.sortActive, this.sortDirection, this.keyword).subscribe(r => this.dataSource = r);
  }

  ngAfterViewInit(): void {
    this.refreshData();
  }

  edit(question: Question) {
    const dialogRef = this.dialog.open(EditQuestionComponent, {
      width: '250px',
      data: question
    });

    dialogRef.afterClosed().subscribe(result => {
      this.refreshData();
    });
  }

  delete(id: number) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: id
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result.data) {
        this.questionService.deleteQuestion(id).subscribe(
          r => {
            this.refreshData();
          }
        );
        this.refreshData();
      }
    });


  }

  addData() {
    const dialogRef = this.dialog.open(AddQuestionComponent, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.refreshData();
    });
  }

  applyFilter($event: KeyboardEvent) {
    this.keyword = ($event.target as HTMLInputElement).value;
    this.refreshData();
  }

  sortData($event: Sort) {
    this.sortActive = $event.active;
    this.sortDirection = $event.direction;
    this.refreshData();
  }
}


