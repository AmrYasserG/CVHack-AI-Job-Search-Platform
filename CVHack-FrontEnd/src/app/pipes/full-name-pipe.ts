import { Pipe, PipeTransform } from '@angular/core';
import { User } from '../models/user.model';


@Pipe({ name: 'fullName' })
export class FullNamePipe implements PipeTransform {
  transform(user: User): string {
    return `${user.firstName} ${user.lastName}`.trim();
  }
}