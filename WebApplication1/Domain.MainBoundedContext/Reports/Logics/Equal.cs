﻿using LinqKit;
using Domain.MainBoundedContext.FilterField;
using System;
using System.Linq.Expressions;
using Domain.MainBoundedContext.Reports.Aggregates;

namespace Domain.MainBoundedContext.Logics
{
    public class Equal<T> :Logic, IFieldLogic<T>
    {
        public AppFilterField<T> Field { get; set; }

        public FieldValue<T> FieldValue { get; set; }

        /*
        public void SetParameterProvider(ParameterProvider pp)
        {
            if (FieldValue is Parameter<T>)
            {
                ((Parameter<T>)(FieldValue)).SetParameterProvider(pp);
            }
        }
        */

        public override Expression<Func<Report, Boolean>> GetExpression(ParameterProvider parameterProvider = null)
        {
            if (FieldValue.GetValue(parameterProvider) == null)
            {
                throw new Exception("The Value for Equal should not be null");
            }

            T value = FieldValue.GetValue(parameterProvider);
            return (this.Field as IEQUAL<T>).GetEqualExpression(value).Expand();
        }
    }
}
