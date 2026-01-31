import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../../endpoints/auth-endpoints/account-security-endpoint.service';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'app-set-security-questions',
  templateUrl: './set-security-questions.component.html',
  styleUrls: ['./set-security-questions.component.scss'],
  standalone:false
})
export class SetSecurityQuestionsComponent implements OnInit {

  questions: any[] = [];
  form!: FormGroup;

  constructor(
    private acct: AccountService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.loadQuestions();
  }

  loadQuestions() {
    this.acct.getAllQuestions().subscribe({
      next: (res: any) => {
        this.questions = res;

        // Pravimo form group dinamički
        const group: any = {
          email: ['', [Validators.required, Validators.email]]
        };

        this.questions.forEach(q => {
          group['question_' + q.questionId] = ['', Validators.required];
        });

        this.form = this.fb.group(group);
      },
      error: () => alert('Cannot load security questions')
    });
  }

  save() {
    if (this.form.invalid) return alert("Fill all fields!");

    const formValue = this.form.value;

    const payload = {
      email: formValue.email,
      answers: this.questions.map(q => ({
        questionId: q.questionId,
        answer: formValue['question_' + q.questionId]
      }))
    };

    this.acct.saveQuestions(payload).subscribe({
      next: () => {
        alert("Security questions saved successfully!");
      },
      error: (e) => {
        alert(e?.error || "Error saving questions");
      }
    });
  }
}