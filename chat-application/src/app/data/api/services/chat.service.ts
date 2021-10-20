/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpResponse, HttpHeaders } from '@angular/common/http';
import { BaseService as __BaseService } from '../base-service';
import { ApiConfiguration as __Configuration } from '../api-configuration';
import { StrictHttpResponse as __StrictHttpResponse } from '../strict-http-response';
import { Observable as __Observable } from 'rxjs';
import { map as __map, filter as __filter } from 'rxjs/operators';

import { OutputMessageDTO } from '../models/output-message-dto';
import { MessagePostWrapper } from '../models/message-post-wrapper';
import { ChatDTO } from '../models/chat-dto';
@Injectable({
  providedIn: 'root',
})
class ChatService extends __BaseService {
  static readonly postApiV01ChatPath = '/api/v0.1/chat';
  static readonly getApiV01ChatUsernamePath = '/api/v0.1/chat/{username}';

  constructor(
    config: __Configuration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Sends message based on provided body data. It also
   * detects malicious attempts to hack system, logs them and
   * logs out the user. Only a person with valid JWT token can access this endpoint.
   * @param body Message to send
   * @return Success
   */
  postApiV01ChatResponse(body?: MessagePostWrapper): __Observable<__StrictHttpResponse<OutputMessageDTO>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = body;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/api/v0.1/chat`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as __StrictHttpResponse<OutputMessageDTO>;
      })
    );
  }
  /**
   * Sends message based on provided body data. It also
   * detects malicious attempts to hack system, logs them and
   * logs out the user. Only a person with valid JWT token can access this endpoint.
   * @param body Message to send
   * @return Success
   */
  postApiV01Chat(body?: MessagePostWrapper): __Observable<OutputMessageDTO> {
    return this.postApiV01ChatResponse(body).pipe(
      __map(_r => _r.body as OutputMessageDTO)
    );
  }

  /**
   * Reads all messages that belong to certain user.
   * User must be logged in and can access only his messages.
   * @param username Username for which messages are requested
   * @return Success
   */
  getApiV01ChatUsernameResponse(username: string): __Observable<__StrictHttpResponse<Array<ChatDTO>>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    let req = new HttpRequest<any>(
      'GET',
      this.rootUrl + `/api/v0.1/chat/${encodeURIComponent(String(username))}`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as __StrictHttpResponse<Array<ChatDTO>>;
      })
    );
  }
  /**
   * Reads all messages that belong to certain user.
   * User must be logged in and can access only his messages.
   * @param username Username for which messages are requested
   * @return Success
   */
  getApiV01ChatUsername(username: string): __Observable<Array<ChatDTO>> {
    return this.getApiV01ChatUsernameResponse(username).pipe(
      __map(_r => _r.body as Array<ChatDTO>)
    );
  }
}

module ChatService {
}

export { ChatService }
