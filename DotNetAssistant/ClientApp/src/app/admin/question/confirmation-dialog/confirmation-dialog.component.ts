import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.css']
})
export class ConfirmationDialogComponent implements OnInit {
  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<ConfirmationDialogComponent>
    , @Inject(MAT_DIALOG_DATA) public id: number) { }

  ngOnInit(): void {
  }

  Yes() {
    this.dialogRef.close({data : true});
  }

  No() {
    this.dialogRef.close({data: false});
  }
}
