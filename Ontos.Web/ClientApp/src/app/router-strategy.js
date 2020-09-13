"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var page_component_1 = require("./pages/page/page.component");
var CustomRouteReuseStrategy = /** @class */ (function () {
    function CustomRouteReuseStrategy() {
    }
    CustomRouteReuseStrategy.prototype.shouldDetach = function (route) { return false; };
    CustomRouteReuseStrategy.prototype.store = function (route, detachedTree) { };
    CustomRouteReuseStrategy.prototype.shouldAttach = function (route) { return false; };
    CustomRouteReuseStrategy.prototype.retrieve = function (route) { return null; };
    CustomRouteReuseStrategy.prototype.shouldReuseRoute = function (future, curr) {
        return curr.component !== page_component_1.PagePage;
    };
    return CustomRouteReuseStrategy;
}());
exports.CustomRouteReuseStrategy = CustomRouteReuseStrategy;
//# sourceMappingURL=router-strategy.js.map