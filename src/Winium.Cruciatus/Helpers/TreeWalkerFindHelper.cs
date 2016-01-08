namespace Winium.Cruciatus.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows.Automation;
    using Winium.Cruciatus.Extensions;

    /// <summary>
    /// A static class to find <see cref="AutomationElement"/> objects using a <see cref="TreeWalker"/>.
    /// </summary>
    internal static class TreeWalkerFindHelper
    {
        /// <summary>
        /// Using the <see cref="TreeWalker.ControlViewWalker"/>, search for the first <see cref="AutomationElement"/> object
        /// based on the <paramref name="element"/> using the specified <paramref name="scope"/> that matches the specified
        /// <paramref name="condition"/>, limited to a maximum of <paramref name="timeout"/> milliseconds.
        /// </summary>
        /// <param name="element">The <see cref="AutomationElement"/> from which to search.</param>
        /// <param name="scope">The scope of the search.</param>
        /// <param name="condition">The condition to match.</param>
        /// <param name="timeout">The maximum amount of milliseconds to search.</param>
        /// <returns>A collection of the <see cref="AutomationElement"/> objects matching the specified <paramref name="condition"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="element"/> or <paramref name="condition"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="timeout"/> is 0 or less.</exception>
        public static AutomationElement FindFirst(AutomationElement element, TreeScope scope, Condition condition, int timeout)
        {
            return FindFirst(TreeWalker.ControlViewWalker, element, scope, condition, timeout);
        }

        /// <summary>
        /// Using an instance of <see cref="TreeWalker"/>, search for the first <see cref="AutomationElement"/> object
        /// based on the <paramref name="element"/> using the specified <paramref name="scope"/> that matches the specified
        /// <paramref name="condition"/>, limited to a maximum of <paramref name="timeout"/> milliseconds.
        /// </summary>
        /// <param name="walker">An instance of <see cref="TreeWalker"/> to walk the control tree.</param>
        /// <param name="element">The <see cref="AutomationElement"/> from which to search.</param>
        /// <param name="scope">The scope of the search.</param>
        /// <param name="condition">The condition to match.</param>
        /// <param name="timeout">The maximum amount of milliseconds to search.</param>
        /// <returns>A collection of the <see cref="AutomationElement"/> objects matching the specified <paramref name="condition"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="walker"/>, <paramref name="element"/>, or <paramref name="condition"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="timeout"/> is 0 or less.</exception>
        public static AutomationElement FindFirst(TreeWalker walker, AutomationElement element, TreeScope scope, Condition condition, int timeout)
        {
            if (walker == null)
            {
                throw new ArgumentNullException("walker");
            }

            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }

            if (timeout <= 0)
            {
                throw new ArgumentOutOfRangeException("timeout", "timeout must be greater than 0");
            }

            Func<AutomationElement, bool> conditionFunction = ConvertToFunction(condition);
            Stopwatch timer = Stopwatch.StartNew();

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (timer.ElapsedMilliseconds <= timeout)
            {
                if (scope.HasFlag(TreeScope.Element) && conditionFunction(element))
                {
                    return element;
                }

                bool hasFlagDescendants = scope.HasFlag(TreeScope.Descendants);
                if (scope.HasFlag(TreeScope.Children) || hasFlagDescendants)
                {
                    IEnumerable<AutomationElement> matchingDescendants = FindChildMatches(walker, element, conditionFunction, timeout, timer, hasFlagDescendants, true);
                    if (matchingDescendants.Any())
                    {
                        return matchingDescendants.First();
                    }
                }

                bool hasFlagAncestors = scope.HasFlag(TreeScope.Ancestors);
                if (scope.HasFlag(TreeScope.Parent) || hasFlagAncestors)
                {
                    if (element != AutomationElement.RootElement)
                    {
                        AutomationElement parent;
                        try
                        {
                            parent = walker.GetParent(element);
                        }
                        catch (ElementNotAvailableException)
                        {
                            parent = null;
                        }

                        while (parent != null && timer.ElapsedMilliseconds <= timeout)
                        {
                            if (conditionFunction(parent))
                            {
                                return parent;
                            }

                            if (!hasFlagAncestors || parent == AutomationElement.RootElement)
                            {
                                break;
                            }

                            try
                            {
                                parent = walker.GetParent(parent);
                            }
                            catch (ElementNotAvailableException)
                            {
                                parent = null;
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Using the <see cref="TreeWalker.ControlViewWalker"/>, search for <see cref="AutomationElement"/> objects
        /// based on the <paramref name="element"/> using the specified <paramref name="scope"/> that match the specified
        /// <paramref name="condition"/>, limited to a maximum of <paramref name="timeout"/> milliseconds.
        /// </summary>
        /// <param name="element">The <see cref="AutomationElement"/> from which to search.</param>
        /// <param name="scope">The scope of the search.</param>
        /// <param name="condition">The condition to match.</param>
        /// <param name="timeout">The maximum amount of milliseconds to search.</param>
        /// <returns>A collection of the <see cref="AutomationElement"/> objects matching the specified <paramref name="condition"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="element"/> or <paramref name="condition"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="timeout"/> is 0 or less.</exception>
        public static IEnumerable<AutomationElement> FindAll(AutomationElement element, TreeScope scope, Condition condition, int timeout)
        {
            return FindAll(TreeWalker.ControlViewWalker, element, scope, condition, timeout);
        }

        /// <summary>
        /// Using an instance of <see cref="TreeWalker"/>, search for <see cref="AutomationElement"/> objects
        /// based on the <paramref name="element"/> using the specified <paramref name="scope"/> that match the specified
        /// <paramref name="condition"/>, limited to a maximum of <paramref name="timeout"/> milliseconds.
        /// </summary>
        /// <param name="walker">An instance of <see cref="TreeWalker"/> to walk the control tree.</param>
        /// <param name="element">The <see cref="AutomationElement"/> from which to search.</param>
        /// <param name="scope">The scope of the search.</param>
        /// <param name="condition">The condition to match.</param>
        /// <param name="timeout">The maximum amount of milliseconds to search.</param>
        /// <returns>A collection of the <see cref="AutomationElement"/> objects matching the specified <paramref name="condition"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="walker"/>, <paramref name="element"/>, or <paramref name="condition"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="timeout"/> is 0 or less.</exception>
        public static IEnumerable<AutomationElement> FindAll(TreeWalker walker, AutomationElement element, TreeScope scope, Condition condition, int timeout)
        {
            if (walker == null)
            {
                throw new ArgumentNullException("walker");
            }

            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }

            if (timeout <= 0)
            {
                throw new ArgumentOutOfRangeException("timeout", "timeout must be greater than 0");
            }

            Func<AutomationElement, bool> conditionFunction = ConvertToFunction(condition);
            Stopwatch timer = Stopwatch.StartNew();
            List<AutomationElement> matches = new List<AutomationElement>();

            if (scope.HasFlag(TreeScope.Element) && conditionFunction(element))
            {
                matches.Add(element);
            }

            if (timer.ElapsedMilliseconds <= timeout)
            {
                bool hasFlagDescendants = scope.HasFlag(TreeScope.Descendants);
                if (scope.HasFlag(TreeScope.Children) || hasFlagDescendants)
                {
                    matches.AddRange(FindChildMatches(walker, element, conditionFunction, timeout, timer, hasFlagDescendants, false));
                }
            }

            if (timer.ElapsedMilliseconds <= timeout)
            {
                bool hasFlagAncestors = scope.HasFlag(TreeScope.Ancestors);
                if (scope.HasFlag(TreeScope.Parent) || hasFlagAncestors)
                {
                    if (element != AutomationElement.RootElement)
                    {
                        AutomationElement parent;
                        try
                        {
                            parent = walker.GetParent(element);
                        }
                        catch (ElementNotAvailableException)
                        {
                            parent = null;
                        }

                        while (parent != null && timer.ElapsedMilliseconds <= timeout)
                        {
                            if (conditionFunction(parent))
                            {
                                matches.Add(parent);
                            }

                            if (!hasFlagAncestors || parent == AutomationElement.RootElement)
                            {
                                break;
                            }

                            try
                            {
                                parent = walker.GetParent(parent);
                            }
                            catch (ElementNotAvailableException)
                            {
                                parent = null;
                            }
                        }
                    }
                }
            }

            return matches;
        }

        /// <summary>
        /// Breadth-first search of the child <see cref="AutomationElement"/> objects of the specified
        /// <paramref name="element"/>, returning only elements that match the <paramref name="conditionFunction"/>.
        /// Optionally includes the descendants based on the <paramref name="isIncludeDescendants"/> flag.
        /// Can also optionally return immediately on the first match.
        /// </summary>
        /// <param name="walker">An instance of <see cref="TreeWalker"/> to walk the control tree.</param>
        /// <param name="element">The <see cref="AutomationElement"/> from which to search.</param>
        /// <param name="conditionFunction">The condition function used to determine if a particular child <see cref="AutomationElement"/> is a match.</param>
        /// <param name="timeout">The maximum amount of milliseconds to search.</param>
        /// <param name="timer">A running <see cref="Stopwatch"/> that is used to check timeout.</param>
        /// <param name="isIncludeDescendants">Whether to return the descendants of the child nodes.</param>
        /// <param name="isReturnFirstMatchOnly">Whether to return immediately upon finding a match.</param>
        /// <returns>A collection of the <see cref="AutomationElement"/> objects that are children of the specified <paramref name="element"/> matching the specified <paramref name="conditionFunction"/>.</returns>
        private static IEnumerable<AutomationElement> FindChildMatches(TreeWalker walker, AutomationElement element, Func<AutomationElement, bool> conditionFunction, int timeout, Stopwatch timer, bool isIncludeDescendants, bool isReturnFirstMatchOnly)
        {
            List<AutomationElement> matches = new List<AutomationElement>();
            IEnumerable<AutomationElement> currentLayer = element.AsSingletonEnumerable();
            do
            {
                currentLayer = GetChildLayer(walker, currentLayer, timeout, timer);
                for (int i = 0; i < currentLayer.Count() && timer.ElapsedMilliseconds <= timeout; i++)
                {
                    AutomationElement child = currentLayer.ElementAt(i);
                    if (conditionFunction(child))
                    {
                        matches.Add(child);

                        if (isReturnFirstMatchOnly)
                        {
                            return matches;
                        }
                    }
                }
            }
            while (isIncludeDescendants && timer.ElapsedMilliseconds <= timeout && currentLayer.Any());

            return matches;
        }

        /// <summary>
        /// Gets all <see cref="AutomationElement"/> objects that are children of the specified collection <paramref name="parentLayer"/>
        /// that represents a level in a tree using the <paramref name=" walker"/>.  Breaks and returns if the <paramref name="timer"/>
        /// indicates the <paramref name="timeout"/> has elasped.
        /// </summary>
        /// <param name="walker">An instance of <see cref="TreeWalker"/> to walk the control tree.</param>
        /// <param name="parentLayer">The collection of <see cref="AutomationElement"/> objects representing the parent nodes for which to retrieve child nodes.</param>
        /// <param name="timeout">The maximum amount of milliseconds to search.</param>
        /// <param name="timer">A running <see cref="Stopwatch"/> that is used to check timeout.</param>
        /// <returns>A collection containing <see cref="AutomationElement"/> objects that are the immediate children of <paramref name="parentLayer"/> in the control tree.</returns>
        private static IEnumerable<AutomationElement> GetChildLayer(TreeWalker walker, IEnumerable<AutomationElement> parentLayer, int timeout, Stopwatch timer)
        {
            List<AutomationElement> childLayer = new List<AutomationElement>();
            for (int i = 0; i < parentLayer.Count() && timer.ElapsedMilliseconds <= timeout; i++)
            {
                childLayer.AddRange(GetChildren(walker, parentLayer.ElementAt(i), timeout, timer));
            }

            return childLayer;
        }

        /// <summary>
        /// Gets all <see cref="AutomationElement"/> objects that are children of the specified <paramref name="parent"/> element using
        /// the <paramref name=" walker"/>.  Breaks and returns if the <paramref name="timer"/> indicates the <paramref name="timeout"/>
        /// has elasped.
        /// </summary>
        /// <param name="walker">An instance of <see cref="TreeWalker"/> to walk the control tree.</param>
        /// <param name="parent">The <see cref="AutomationElement"/> object from which to retrieve child nodes.</param>
        /// <param name="timeout">The maximum amount of milliseconds to search.</param>
        /// <param name="timer">A running <see cref="Stopwatch"/> that is used to check timeout.</param>
        /// <returns>A collection containing <see cref="AutomationElement"/> objects that are the immediate children of <paramref name="parentLayer"/> in the control tree.</returns>
        private static IEnumerable<AutomationElement> GetChildren(TreeWalker walker, AutomationElement parent, int timeout, Stopwatch timer)
        {
            List<AutomationElement> children = new List<AutomationElement>();
            AutomationElement child;
            try
            {
                child = walker.GetFirstChild(parent);
            }
            catch (ElementNotAvailableException)
            {
                child = null;
            }

            while (child != null && timer.ElapsedMilliseconds <= timeout)
            {
                children.Add(child);

                try
                {
                    child = walker.GetNextSibling(child);
                }
                catch (ElementNotAvailableException)
                {
                    child = null;
                }
            }

            return children;
        }

        /// <summary>
        /// Converts the AutomationFramework <paramref name="condition"/> into a function that accepts an
        /// <see cref="AutomationElement"/> and returns a <see cref="Boolean"/> indicating whether the element
        /// satisfies the condition.
        /// </summary>
        /// <param name="condition">The <see cref="Condition"/> to convert into a function.</param>
        /// <returns>A function that evaluates whether arbitrary instances of <see cref="AutomationElement"/> match the <paramref name="condition"/>.</returns>
        /// <exception cref="InvalidOperationException">If the <paramref name="condition"/> is not a known AutomationFramework <see cref="Condition"/> or subclass thereof.</exception>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "\"or\" is a literal word describing the purpose of the object.")]
        private static Func<AutomationElement, bool> ConvertToFunction(Condition condition)
        {
            if (condition == Condition.TrueCondition)
            {
                return ae => true;
            }
            else if (condition == Condition.FalseCondition)
            {
                return ae => false;
            }

            Type type = condition.GetType();
            if (IsInstanceOrSubclass<AndCondition>(type))
            {
                AndCondition andCondition = (AndCondition)condition;
                IEnumerable<Func<AutomationElement, bool>> childFunctions = andCondition
                    .GetConditions()
                    .Select(c => ConvertToFunction(c))
                    .ToList();

                return ae => childFunctions.All(c => c(ae));
            }
            else if (IsInstanceOrSubclass<OrCondition>(type))
            {
                OrCondition orCondition = (OrCondition)condition;
                IEnumerable<Func<AutomationElement, bool>> childFunctions = orCondition
                    .GetConditions()
                    .Select(c => ConvertToFunction(c))
                    .ToList();

                return ae => childFunctions.Any(c => c(ae));
            }
            else if (IsInstanceOrSubclass<NotCondition>(type))
            {
                NotCondition notCondition = (NotCondition)condition;

                return ae => !ConvertToFunction(notCondition.Condition)(ae);
            }
            else if (IsInstanceOrSubclass<PropertyCondition>(type))
            {
                return ConvertPropertyConditionToFunction((PropertyCondition)condition);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Unrecognized condition subclass: \"{0}\"", condition.GetType().FullName));
            }
        }

        /// <summary>
        /// Converts the AutomationFramework <paramref name="condition"/> into a function that accepts an
        /// <see cref="AutomationElement"/> and returns a <see cref="Boolean"/> indicating whether the element
        /// satisfies the property condition.
        /// </summary>
        /// <param name="condition">The <see cref="PropertyCondition"/> to convert into a function.</param>
        /// <returns>A function that evaluates whether arbitrary instances of <see cref="AutomationElement"/> match the <paramref name="condition"/>.</returns>
        /// <remarks>This is a separate method from the main conversion method because it is more complex.</remarks>
        private static Func<AutomationElement, bool> ConvertPropertyConditionToFunction(PropertyCondition condition)
        {
            return ae =>
            {
                // null elements do not have any properties to match
                if (ae == null)
                {
                    return false;
                }

                try
                {
                    // elements that do not support the desired property cannot match it
                    if (!ae.GetSupportedProperties().Contains(condition.Property))
                    {
                        return false;
                    }

                    bool isConditionCaseInsensitive = condition.Flags.HasFlag(PropertyConditionFlags.IgnoreCase);
                    object propertyValue = ae.GetCurrentPropertyValue(condition.Property);

                    // if either value is null, both must be null to match
                    bool isConditionValueNull = condition.Value == null;
                    bool isPropertyValueNull = propertyValue == null;
                    if (isConditionValueNull || isPropertyValueNull)
                    {
                        return isConditionValueNull && isPropertyValueNull;
                    }

                    Type propertyValueType = propertyValue.GetType();
                    Type conditionValueType = condition.Value.GetType();
                    bool isConditionValueConvertibleToPropertyType;
                    if (propertyValueType.IsValueType && propertyValueType.IsPrimitive)
                    {
                        // primitives that can be converted can be compared
                        isConditionValueConvertibleToPropertyType = IsConvertibleToPrimitive(condition.Value, propertyValueType);
                    }
                    else if (propertyValueType.IsEnum) // enumerations are value types, but not primitives
                    {
                        if (conditionValueType == typeof(string)) // enums can be compared to their string representation
                        {
                            try
                            {
                                object enumValue = Enum.Parse(propertyValueType, condition.Value as string, isConditionCaseInsensitive);
                                isConditionValueConvertibleToPropertyType = true;
                            }
                            catch
                            {
                                isConditionValueConvertibleToPropertyType = false;
                            }
                        }
                        else if (conditionValueType.IsEnum) // enumerations of the same type may be compared
                        {
                            isConditionValueConvertibleToPropertyType = conditionValueType == propertyValueType;
                        }
                        else if (IsConvertibleToPrimitive(condition.Value, Enum.GetUnderlyingType(propertyValueType))) // enumerations can be compared to their underlying value types
                        {
                            isConditionValueConvertibleToPropertyType = true;
                        }
                        else
                        {
                            isConditionValueConvertibleToPropertyType = false;
                        }
                    }
                    else if (propertyValueType.IsAssignableFrom(conditionValueType))
                    {
                        isConditionValueConvertibleToPropertyType = true;
                    }
                    else
                    {
                        isConditionValueConvertibleToPropertyType = false;
                    }

                    // condition and property values that cannot be compared cannot be tested for equality, and thus do not match
                    if (!isConditionValueConvertibleToPropertyType)
                    {
                        return false;
                    }
                    else if (propertyValueType == typeof(string) && conditionValueType == typeof(string))
                    {
                        // test strings for equality based on case-sensitivity flag
                        return string.Equals(
                            (string)propertyValue,
                            (string)condition.Value,
                            isConditionCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                    }
                    else
                    {
                        return propertyValue == condition.Value;
                    }
                }
                catch (ElementNotAvailableException) // elements that do not exist cannot match a condition
                {
                    return false;
                }
            };
        }

        /// <summary>
        /// Determines whether <paramref name="instanceType"/> is of type <typeparamref name="TClassOrSuper"/> or a subclass of it.
        /// </summary>
        /// <typeparam name="TClassOrSuper">The <see cref="Type"/> of the class/super-class (must be a reference type).</typeparam>
        /// <param name="instanceType">The <see cref="Type"/> of the instance to check.</param>
        /// <returns><c>true</c> if <paramref name="instanceType"/> is type <typeparamref name="TClassOrSuper"/> or a subclass of it; otherwise <c>false</c>.</returns>
        private static bool IsInstanceOrSubclass<TClassOrSuper>(Type instanceType) where TClassOrSuper : class
        {
            return instanceType == typeof(TClassOrSuper) || instanceType.IsSubclassOf(typeof(TClassOrSuper));
        }

        /// <summary>
        /// Determines whether an instance of <see cref="Type"/> represents a <c>null</c>-able type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to test.</param>
        /// <returns><c>true</c> if <paramref name="type"/> is <c>null</c>-able; otherwise <c>false</c>.</returns>
        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="instance"/> can be converted to the <paramref name="target"/> <see cref="Type"/>.
        /// </summary>
        /// <param name="instance">The value to test for convertibility.</param>
        /// <param name="target">The desired primitive <see cref="Type"/>.</param>
        /// <returns><c>true</c> if the <paramref name="instance"/> can be converted to the specified <paramref name="target"/> <see cref="Type"/>; otherwise <c>false</c>.</returns>
        private static bool IsConvertibleToPrimitive(object instance, Type target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (!target.IsValueType || !target.IsPrimitive)
            {
                return false;
            }

            try
            {
                // attempt to convert the value to the desired type - success indicates they can be compared
                Convert.ChangeType(instance, target);
                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
        }
    }
}