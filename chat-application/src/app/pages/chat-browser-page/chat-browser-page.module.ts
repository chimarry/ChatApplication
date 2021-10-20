import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatBrowserComponent } from './chat-browser/chat-browser.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [ChatBrowserComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class ChatBrowserPageModule { }
