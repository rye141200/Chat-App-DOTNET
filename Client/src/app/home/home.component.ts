import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from '../register/register.component';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;
  private http = inject(HttpClient);
  private accountServices = inject(AccountService);
  ngOnInit(): void {
    this.getUsers();
    this.setCurrentUser();
  }
  registerToggle() {
    this.registerMode = !this.registerMode;
  }
  getUsers() {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: (response) => (this.users = response),
      error: (err) => console.log(err),
      complete: () => {
        console.log('Success ✅');
        //! no need to unsubscribe
      },
    });
  }
  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user = JSON.parse(userString);
    this.accountServices.currentUser.set(user);
  }
  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
}
