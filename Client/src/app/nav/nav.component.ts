import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
//import { NgIf } from '@angular/common';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
//! We have two types of forms in angular:
//! 1) Template forms, thats right the normal forms
//! 2) Angular forms, where the logic is inside angular not the template (html)
//! There is also reactive forms but we wont use it here
export class NavComponent {
  accountService = inject(AccountService);
  model: any = {};
  login() {
    // console.log(this.model);
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (err) => console.log(err),
    });
  }

  logout() {
    this.accountService.logout();
  }
}
