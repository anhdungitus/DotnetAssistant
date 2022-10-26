import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from "@angular/forms";
import {Question, QuestionService} from "../question.service";
import {MatDialogRef} from "@angular/material/dialog";
import {MAT_DIALOG_DATA} from '@angular/material/dialog';


@Component({
  selector: 'app-edit-question',
  templateUrl: './edit-question.component.html',
  styleUrls: ['./edit-question.component.css']
})
export class EditQuestionComponent implements OnInit {
  questionForm = this.fb.group({
    id: new FormControl(''),
    text: new FormControl('')
  });

  constructor(private fb: FormBuilder, private questionService: QuestionService, public dialogRef: MatDialogRef<EditQuestionComponent>
  , @Inject(MAT_DIALOG_DATA) public data: Question) { }

  ngOnInit(): void {
    this.questionForm.setValue({
      id: this.data.id,
      text: this.data.text
    })
  }

  onSubmit() {
    this.questionService.update(this.questionForm.value).subscribe(
      result => {
        if(result) {
          this.dialogRef.close();
        }
      }
    );
  }
}
