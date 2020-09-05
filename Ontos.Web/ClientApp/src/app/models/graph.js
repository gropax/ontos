"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Page = /** @class */ (function () {
    function Page(id, content, references) {
        if (references === void 0) { references = []; }
        this.id = id;
        this.content = content;
        this.references = references;
    }
    return Page;
}());
exports.Page = Page;
var Reference = /** @class */ (function () {
    function Reference(id, pageId, expression) {
        this.id = id;
        this.pageId = pageId;
        this.expression = expression;
    }
    return Reference;
}());
exports.Reference = Reference;
var Expression = /** @class */ (function () {
    function Expression(id, language, label) {
        this.id = id;
        this.language = language;
        this.label = label;
    }
    return Expression;
}());
exports.Expression = Expression;
var NewPage = /** @class */ (function () {
    function NewPage(content, expression) {
        if (expression === void 0) { expression = null; }
        this.content = content;
        this.expression = expression;
    }
    return NewPage;
}());
exports.NewPage = NewPage;
var NewExpression = /** @class */ (function () {
    function NewExpression(language, label) {
        this.language = language;
        this.label = label;
    }
    return NewExpression;
}());
exports.NewExpression = NewExpression;
var NewReference = /** @class */ (function () {
    function NewReference(pageId, expression) {
        this.pageId = pageId;
        this.expression = expression;
    }
    return NewReference;
}());
exports.NewReference = NewReference;
var UpdatePage = /** @class */ (function () {
    function UpdatePage(id, content) {
        this.id = id;
        this.content = content;
    }
    return UpdatePage;
}());
exports.UpdatePage = UpdatePage;
//# sourceMappingURL=graph.js.map