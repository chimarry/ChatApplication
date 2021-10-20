import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ChatDTO, OutputMessageDTO } from 'src/app/data/api/models';
import { ChatService, UserService } from 'src/app/data/api/services';
import { AuthService } from 'src/common/auth-service';

@Component({
  selector: 'app-chat-browser',
  templateUrl: './chat-browser.component.html',
  styleUrls: ['./chat-browser.component.css']
})
export class ChatBrowserComponent implements OnInit {

  selectedMessages: Array<OutputMessageDTO>;
  @ViewChild('msgContainer') private msgContainer: ElementRef;

  username: string;

  chatList: ChatDTO[];
  selectedUsername: string = '';

  chatForm = this.formBuilder.group({
    message: ''
  });

  requestInProgress: boolean = false;

  constructor(
    private chatService: ChatService,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.requestInProgress = true;
    this.refreshChatPeriodically(6000);
    this.refreshTokenPeriodically(40000);
  }

  getMessageType(message: OutputMessageDTO) {
    if(message.senderUsername === this.username) {
      return "Sent";
    } else {
      return "Received"
    }
  }

  refreshChat() {
    this.username = JSON.parse(localStorage.getItem('userName'));
    this.chatService.getApiV01ChatUsername(this.username)
      .pipe(
        catchError(event => {
          return throwError(event);
        }))
      .subscribe(chList => {
          this.chatList = chList;
          if(this.selectedUsername === '' && this.chatList.length > 0) {
            this.selectedUsername = this.chatList[0].username;
          }
          this.chatList.forEach(element => {
            if(this.isUserSelected(element.username)) {
              this.selectedMessages = element.messages
            }
          });
          this.requestInProgress = false;
        });
  }

  scrollToBottom() {
    try {
        this.msgContainer.nativeElement.scrollTop = this.msgContainer.nativeElement.scrollHeight;
    } catch(err) { }                 
  }

  isUserSelected(username) {
    return username === this.selectedUsername;
  }

  selectUser(username) {
    this.selectedUsername = username;
    for (let element of this.chatList){
      if(this.isUserSelected(element.username)) {
        this.selectedMessages = element.messages;
      }
    }
    this.scrollToBottom();
  }

  sendMessage() {
    this.chatService.postApiV01Chat({
      senderUsername: this.username,
      receiverUsername: this.selectedUsername,
      text: this.chatForm.value.message
    }).pipe(
      catchError(event => {
        return throwError(event);
      })
    ).subscribe(res => {
        this.refreshChat();
        this.scrollToBottom();
        this.chatForm = this.formBuilder.group({
          message: ''
        });
      });
  }

  refreshChatPeriodically(delay: number) {
    return new Promise(async resolve => {
      while (this.authService.isLoggedIn()) {
        this.refreshChat();
        await this.sleep(delay);
      }
      resolve(true);
    });
  }

  refreshTokenPeriodically(delay: number) {
    return new Promise(async resolve => {
      await this.sleep(delay);
      while (this.authService.isLoggedIn()) {
        this.authService.refreshToken();
        await this.sleep(delay);
      }
      resolve(true);
    });
  }

  sleep(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
  }

}
