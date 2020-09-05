import { trigger, animate, transition, style, query } from '@angular/animations';

export const fadeAnimation =
  trigger('fadeAnimation', [
    transition('* => *', [
      style({ position: 'relative' }),
      query(':enter, :leave', [
        style({
          position: 'absolute',
          top: -16,  // without this, page moves downward during animation.
          width: '100%',
          height: '100%',
        }),
      ], { optional: true }),
      query(':enter', [
        style({ opacity: 0 })
      ], { optional: true }),
      query(':leave', [
        style({ opacity: 1 }),
        animate('0.2s', style({ opacity: 0 }))
      ], { optional: true }),
      query(':enter', [
        style({ opacity: 0 }),
        animate('0.2s', style({ opacity: 1 }))
      ], { optional: true })
    ])
  ]);
