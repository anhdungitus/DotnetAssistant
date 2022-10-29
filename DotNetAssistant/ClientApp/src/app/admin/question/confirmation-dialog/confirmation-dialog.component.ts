import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder} from "@angular/forms";
import {QuestionService} from "../question.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.css']
})
export class ConfirmationDialogComponent implements OnInit {
  constructor(private fb: FormBuilder, private questionService: QuestionService, public dialogRef: MatDialogRef<ConfirmationDialogComponent>
    , @Inject(MAT_DIALOG_DATA) public id: number) { }

  ngOnInit(): void {
  }

  Yes() {
    this.dialogRef.close({data : true});
    // this.questionService.deleteQuestion(this.id).subscribe(s => {
    // });
  }

  No() {
    this.dialogRef.close({data: false});
  }
}
