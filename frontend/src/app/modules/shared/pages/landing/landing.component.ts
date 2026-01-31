import { ChangeDetectionStrategy, Component, computed, signal } from '@angular/core';
//import {AnimateOnScrollDirective} from '../../directives/animate-on-scroll.directive';

interface Feature {
  icon: string;
  title: string;
  description: string;
  imageUrl: string;
}

interface Announcement {
  title: string;
  date: string;
  content: string;
}

@Component({
  selector: 'app-landing',
  standalone: false,
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css'],
  //imports: [AnimateOnScrollDirective],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LandingPageComponent {

  features: Feature[] = [
    {
      icon: `<svg xmlns="http://www.w3.org/2000/svg" class="h-12 w-12 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002 2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>`,
      title: 'Online Dorm Application',
      description: 'Apply for your dorm room online with our easy-to-use application form. Submit documents and track your application status seamlessly.',
      imageUrl: 'images/papers.jpg'
    },
    {
      icon: `<svg xmlns="http://www.w3.org/2000/svg" class="h-12 w-12 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" /></svg>`,
      title: 'Maintenance Requests',
      description: 'Report issues directly through the app. Track the status of your request from submission to completion.',
      imageUrl: 'images/maintenance.jpg'
    },
    {
      icon: `<svg xmlns="http://www.w3.org/2000/svg" class="h-12 w-12 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5.882V19.24a1.76 1.76 0 01-3.417.592l-2.147-6.15M18 13a3 3 0 100-6M5.436 13.683A4.001 4.001 0 017 6h1.832c4.1 0 7.625-2.236 9.168-5.584C18.354 1.832 18.666 2 18.997 2c.389 0 .748-.158 1.023-.421V2.5" /></svg>`,
      title: 'Community Announcements',
      description: 'Stay updated with important notices, events, and community news from the dormitory administration.',
      imageUrl: 'images/announcement.jpg'
    }
  ];

  announcements: Announcement[] = [
    {
      title: 'Welcome Week 2024 Schedule',
      date: 'August 15, 2024',
      content: 'Get ready for an exciting welcome week! We have a series of events planned to help you settle in and meet new people. Check the student portal for the full schedule.'
    },
    {
      title: 'Quarterly Room Inspections',
      date: 'August 12, 2024',
      content: 'Please be advised that routine room inspections will be conducted from August 20th to August 22nd. We appreciate your cooperation in keeping our community safe.'
    },
    {
      title: 'New Meal Plan Options Available',
      date: 'August 10, 2024',
      content: 'We are excited to introduce new flexible meal plan options for the upcoming semester. Visit the dining services office to learn more and make changes to your plan.'
    }
  ];

  currentAnnouncementIndex = signal(0);

  // Return as a single-item array to help with @for and animations
  visibleAnnouncement = computed(() => [this.announcements[this.currentAnnouncementIndex()]]);

  nextAnnouncement(): void {
    this.currentAnnouncementIndex.update(index => (index + 1) % this.announcements.length);
  }

  prevAnnouncement(): void {
    this.currentAnnouncementIndex.update(index => (index - 1 + this.announcements.length) % this.announcements.length);
  }
}
