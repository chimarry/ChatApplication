/* tslint:disable */
import { OutputMessageDTO } from './output-message-dto';
export interface ChatDTO {
  messages?: null | Array<OutputMessageDTO>;
  username?: null | string;
}
