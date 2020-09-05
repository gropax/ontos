"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var animations_1 = require("@angular/animations");
exports.fadeAnimation = animations_1.trigger('fadeAnimation', [
    animations_1.transition('* => *', [
        animations_1.style({ position: 'relative' }),
        animations_1.query(':enter, :leave', [
            animations_1.style({
                position: 'absolute',
                //top: -16,  // without this, page moves downward during animation.
                width: '100%',
                height: '100%',
            }),
        ], { optional: true }),
        animations_1.query(':enter', [
            animations_1.style({ opacity: 0 })
        ], { optional: true }),
        animations_1.query(':leave', [
            animations_1.style({ opacity: 1 }),
            animations_1.animate('0.2s', animations_1.style({ opacity: 0 }))
        ], { optional: true }),
        animations_1.query(':enter', [
            animations_1.style({ opacity: 0 }),
            animations_1.animate('0.2s', animations_1.style({ opacity: 1 }))
        ], { optional: true })
    ])
]);
//# sourceMappingURL=animations.js.map