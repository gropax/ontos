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
var Relation = /** @class */ (function () {
    function Relation(id, type, originId, targetId) {
        this.id = id;
        this.type = type;
        this.originId = originId;
        this.targetId = targetId;
    }
    return Relation;
}());
exports.Relation = Relation;
var RelatedPage = /** @class */ (function () {
    function RelatedPage(id, type, originId, target) {
        this.id = id;
        this.type = type;
        this.originId = originId;
        this.target = target;
    }
    return RelatedPage;
}());
exports.RelatedPage = RelatedPage;
var RelationType = /** @class */ (function () {
    function RelationType(label, directed, acyclic) {
        this.label = label;
        this.directed = directed;
        this.acyclic = acyclic;
    }
    RelationType.INCLUSION = new RelationType('INCLUDE', true, true);
    RelationType.INTERSECTION = new RelationType('INTERSECT', false, false);
    return RelationType;
}());
exports.RelationType = RelationType;
var DirectedRelationType = /** @class */ (function () {
    function DirectedRelationType(label, type, reversed) {
        this.label = label;
        this.type = type;
        this.reversed = reversed;
    }
    DirectedRelationType.all = function () {
        return [this.INCLUDES, this.INCLUDED_IN, this.INTERSECTS];
    };
    DirectedRelationType.INCLUDES = new DirectedRelationType("Includes", RelationType.INCLUSION, false);
    DirectedRelationType.INCLUDED_IN = new DirectedRelationType("Is included in", RelationType.INCLUSION, true);
    DirectedRelationType.INTERSECTS = new DirectedRelationType("Intersects", RelationType.INTERSECTION, false);
    return DirectedRelationType;
}());
exports.DirectedRelationType = DirectedRelationType;
var NewPage = /** @class */ (function () {
    function NewPage(content, expression) {
        if (expression === void 0) { expression = null; }
        this.content = content;
        this.expression = expression;
    }
    return NewPage;
}());
exports.NewPage = NewPage;
var PageSearch = /** @class */ (function () {
    function PageSearch(language, text) {
        this.language = language;
        this.text = text;
    }
    return PageSearch;
}());
exports.PageSearch = PageSearch;
var PageSearchResult = /** @class */ (function () {
    function PageSearchResult(pageId, score, expressions) {
        this.pageId = pageId;
        this.score = score;
        this.expressions = expressions;
    }
    return PageSearchResult;
}());
exports.PageSearchResult = PageSearchResult;
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
var NewRelation = /** @class */ (function () {
    function NewRelation(type, originId, targetId) {
        this.type = type;
        this.originId = originId;
        this.targetId = targetId;
    }
    NewRelation.create = function (type, originId, targetId) {
        var _a;
        if (type.reversed)
            _a = [targetId, originId], originId = _a[0], targetId = _a[1];
        return new NewRelation(type.type, originId, targetId);
    };
    NewRelation.prototype.toParams = function () {
        return {
            type: this.type.label, originId: this.originId, targetId: this.targetId,
        };
    };
    return NewRelation;
}());
exports.NewRelation = NewRelation;
var NewRelatedPage = /** @class */ (function () {
    function NewRelatedPage(type, expression, content) {
        this.type = type;
        this.expression = expression;
        this.content = content;
    }
    NewRelatedPage.prototype.toParams = function () {
        return {
            type: this.type.type.label,
            reversed: this.type.reversed,
            expression: this.expression,
            content: this.content,
        };
    };
    return NewRelatedPage;
}());
exports.NewRelatedPage = NewRelatedPage;
var UpdatePage = /** @class */ (function () {
    function UpdatePage(id, content) {
        this.id = id;
        this.content = content;
    }
    return UpdatePage;
}());
exports.UpdatePage = UpdatePage;
//# sourceMappingURL=graph.js.map