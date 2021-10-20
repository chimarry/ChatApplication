import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VerificationCodeComponent } from './verification-code/verification-code.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [VerificationCodeComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class VerificationCodePageModule { }
