import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../../../endpoints/auth-endpoints/account-security-endpoint.service';

@Component({
  selector: 'app-security-questions',
  templateUrl: './security-questions.component.html',
  styleUrls: ['./security-questions.component.scss'],
  standalone:false
})
export class SecurityQuestionsComponent implements OnInit {

  form!: FormGroup;
  questions = [
    "What is your mother's maiden name?",
    "What was the name of your first pet?",
    "In which city were you born?",
    "What is the name of your primary school?",
    "What is your favorite movie?"
  ];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      question1: ['', Validators.required],
      answer1: ['', Validators.required],

      question2: ['', Validators.required],
      answer2: ['', Validators.required],

      question3: ['', Validators.required],
      answer3: ['', Validators.required]
    });
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.accountService.saveQuestions(this.form.value).subscribe({
      next: () => {
        alert("Security questions saved!");
        this.router.navigate(['/dashboard']);
      },
      error: () => alert("Error saving security questions")
    });
  }
}