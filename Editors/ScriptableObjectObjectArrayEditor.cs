﻿// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEditor;
// using UnityEditorInternal;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace UnityBoosts.Editors
// {
//     [CustomEditor(typeof(ScriptableObjectObjectArray))]
//     public class ScriptableObjectObjectArrayEditor : Editor
//     {
//         private ScriptableObjectObjectArrayArray[] _database;
//         private bool _databaseFoldout;
//         private HashSet<Object> _hashSet;
//         private int _length;
//         private Object[] _objects;
//         private bool _objectFoldout;
//         private ScriptableObjectObjectArray _target;
//         private SerializedProperty _valuesProperties;
//         private ReorderableList _values;
//
//         private void OnEnable()
//         {
//             _database = AssetDatabase.FindAssets($"t:{typeof(ScriptableObjectObjectArrayArray)}")
//                 .Select(it =>
//                     AssetDatabase.LoadAssetAtPath<ScriptableObjectObjectArrayArray>(AssetDatabase.GUIDToAssetPath(it)))
//                 .ToArray();
//             _target = target as ScriptableObjectObjectArray ?? throw new NullReferenceException(nameof(target));
//             _hashSet = _target.ToHashSet();
//             _objects = AssetDatabase.FindAssets("t:Prefab")
//                 .Select(it => AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(it), _target.Type))
//                 .Where(it => it).ToArray();
//             _valuesProperties = serializedObject.FindProperty("values");
//             _values = new ReorderableList(serializedObject, _valuesProperties, true, true, true, true)
//             {
//                 drawHeaderCallback = DrawHeader,
//                 drawElementCallback = DrawElement,
//                 elementHeightCallback = ElementHeight
//             };
//         }
//
//         public override void OnInspectorGUI()
//         {
//             var typeProp = serializedObject.FindProperty("type");
//             if (_database == null || _database.Length == 0)
//             {
//                 EditorGUILayout.HelpBox("You must be create object database", MessageType.Warning);
//             }
//             else
//             {
//                 _databaseFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_databaseFoldout, "Database");
//                 if (_databaseFoldout)
//                 {
//                     foreach (var database in _database)
//                     {
//                         var dirty = false;
//                         var map = database.ToDictionary(it => it.objects.GetInstanceID());
//                         var contain = map.TryGetValue(_target.GetInstanceID(), out var pair);
//                         EditorGUILayout.BeginHorizontal();
//                         EditorGUILayout.ObjectField(database, typeof(ScriptableObjectObjectArrayArray), false);
//                         var toggle = EditorGUILayout.Toggle(contain);
//                         EditorGUILayout.EndHorizontal();
//                         if (toggle)
//                         {
//                             var hash = string.IsNullOrEmpty(pair.hash)
//                                 ? ScriptableObjectObjectArray.GetHash(_target.Type)
//                                 : pair.hash;
//                             var newHash = EditorGUILayout.TextField(hash);
//                             if (!map.ContainsKey(_target.GetInstanceID()) || hash != newHash)
//                             {
//                                 dirty = true;
//                                 map[_target.GetInstanceID()] = new ScriptableObjectObjectArrayArray.Data
//                                 {
//                                     hash = newHash,
//                                     objects = _target
//                                 };
//                                 database.SetValue(map.Values.OrderBy(it => it.hash));
//                             }
//                         }
//                         else
//                         {
//                             if (map.Remove(_target.GetInstanceID()))
//                             {
//                                 dirty = true;
//                             }
//                         }
//                         if (!dirty) continue;
//                         database.SetValue(map.Values.OrderBy(it => it.hash));
//                         EditorUtility.SetDirty(database);
//                         AssetDatabase.SaveAssetIfDirty(database);
//                     }
//                 }
//
//                 EditorGUILayout.EndFoldoutHeaderGroup();
//             }
//
//             EditorGUILayout.PropertyField(typeProp);
//             if (_target.Type == null) return;
//             _valuesProperties.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(
//                 _valuesProperties.isExpanded,
//                 "Values"
//             );
//             if (_valuesProperties.isExpanded)
//             {
//                 _values.DoLayoutList();
//             }
//
//             EditorGUILayout.EndFoldoutHeaderGroup();
//             _objectFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_objectFoldout, "Objects");
//             if (_objectFoldout)
//             {
//                 _hashSet = _target.ToHashSet();
//                 foreach (var it in _objects)
//                 {
//                     EditorGUILayout.BeginHorizontal();
//                     EditorGUILayout.ObjectField(it, _target.Type, false);
//                     var contain = _hashSet.Contains(it);
//                     if (EditorGUILayout.Toggle(contain))
//                     {
//                         if (!contain)
//                         {
//                             _hashSet.Add(it);
//                             _target.Set(_hashSet);
//                             EditorUtility.SetDirty(_target);
//                         }
//                     }
//                     else
//                     {
//                         if (contain)
//                         {
//                             _hashSet.Remove(it);
//                             _target.Set(_hashSet);
//                             EditorUtility.SetDirty(_target);
//                         }
//                     }
//
//                     EditorGUILayout.EndHorizontal();
//                 }                
//             }
//             EditorGUILayout.EndFoldoutHeaderGroup();
//             AssetDatabase.SaveAssetIfDirty(_target);
//         }
//
//         private void DrawHeader(Rect rect)
//         {
//             // _valuesProperties.isExpanded = EditorGUI.Foldout(rect, _valuesProperties.isExpanded, "Values");
//             EditorGUI.PropertyField(rect, _valuesProperties.FindPropertyRelative("Array.size"));
//         }
//
//         private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
//         {
//             if (!_valuesProperties.isExpanded) return;
//             var element = _values.serializedProperty.GetArrayElementAtIndex(index);
//             EditorGUI.ObjectField(rect, element, _target.Type);
//         }
//
//         private float ElementHeight(int index)
//         {
//             return _valuesProperties.isExpanded
//                 ? EditorGUI.GetPropertyHeight(_values.serializedProperty.GetArrayElementAtIndex(index))
//                 : 0;
//         }
//     }
// }