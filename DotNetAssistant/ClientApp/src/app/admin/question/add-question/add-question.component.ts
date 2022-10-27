import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from "@angular/forms";
import {Question, QuestionService} from "../question.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-add-question',
  templateUrl: './add-question.component.html',
  styleUrls: ['./add-question.component.css']
})
export class AddQuestionComponent implements OnInit {

  questionForm = this.fb.group({
    text: new FormControl('')
  });

  constructor(private fb: FormBuilder, private questionService: QuestionService, public dialogRef: MatDialogRef<AddQuestionComponent>
    , @Inject(MAT_DIALOG_DATA) public data: Question) { }

  ngOnInit() {}

  onSubmit() {
    this.questionService.add(this.questionForm.value).subscribe(
      result => {
        if(result) {
          this.dialogRef.close();
        }
      }
    );
  }
}
