using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityUtils
{
    #region Attribute 선언부

    /// <summary>
    /// Awake()에서 현재 스크립트가 부착된 오브젝트(부모)를 기준으로 <b>하위 오브젝트</b>를 탐색해 변수에 자동으로 연결합니다.
    /// </summary>
    /// <remarks>
    /// <para><b>[사용 규칙]</b></para>
    /// <list type="number">
    /// <item>변수는 <c>private</c>이어야 하며, <c>static</c>일 수 없습니다.</item>
    /// <item>변수 이름은 <b>CamelCase</b>여야 하며, 탐색 시 언더바(_)는 무시됩니다. (예: <c>_loginButton</c>)</item>
    /// <item>오브젝트 이름은 <b>[접두어 + PascalCase 변환 이름]</b>입니다. (예: <c>Button_LoginButton</c>)</item>
    /// <item>접두어는 <b>[유니티 기본 생성 이름 + 언더바]</b>입니다. (예: <c>Text (TMP)_</c>, <c>Button_</c>)</item>
    /// <item>단, <c>GameObject</c>, <c>Transform</c>, <c>RectTransform</c> 및 기타 사용자 정의 스크립트는 접두어를 붙이지 않습니다.</item>
    /// </list>
    /// <para><b>[예시]</b></para>
    /// <code>
    /// [Bind] private Button _loginButton; 
    /// // 탐색 대상: "Button_LoginButton" (자동 변환)
    /// 
    /// [Bind("_loginButton")] private Button _btn1; 
    /// // 탐색 대상: "Button_LoginButton" (SearchName 사용 시에도 접두어/Pascal 규칙 적용)
    /// 
    /// [Bind("MyCustomObj")] private GameObject _obj; 
    /// // 탐색 대상: "MyCustomObj" (GameObject는 접두어 없음)
    /// </code>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)] // 필드(변수)에만 붙일 수 있음
    public class BindAttribute : Attribute
    {
        public string SearchName { get; private set; }
        public BindAttribute() { SearchName = null; }
        public BindAttribute(string searchName) { SearchName = searchName; }
    }

    /// <summary>
    /// Awake()에서 특정 부모 오브젝트를 찾고, 그 부모의 <b>직속 자식들</b>을 배열이나 리스트에 한꺼번에 연결합니다.
    /// </summary>
    /// <remarks>
    /// <para><b>[사용 규칙]</b></para>
    /// <list type="number">
    /// <item>변수는 <c>Array</c> 또는 <c>List&lt;T&gt;</c> 타입이어야 하며, <c>private</c>이어야 합니다.</item>
    /// <item><b>부모 오브젝트</b>의 이름은 [Bind]와 동일하게 <b>[접두어 + PascalCase 변환 이름]</b>으로 탐색합니다.</item>
    /// <item>부모를 찾으면, 자식의 이름과 상관없이 <b>모든 직속 자식</b>의 컴포넌트를 가져옵니다.</item>
    /// </list>
    /// <para><b>[예시]</b></para>
    /// <code>
    /// [BindList] private Button[] _stageButtons;
    /// // 변수명(_stageButtons) 기반 부모 탐색: "Button_StageButtons"
    /// // 해당 부모 아래의 모든 Button 컴포넌트를 배열에 연결
    /// 
    /// [BindList("MyGrid")] private Transform[] _gridSlots;
    /// // SearchName 사용 시 부모 탐색: "MyGrid" (접두어 무시/포함 여부는 Bind 규칙 따름)
    /// // 해당 부모 아래의 모든 Transform을 배열에 연결
    /// </code>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class BindListAttribute : Attribute
    {
        public string SearchName { get; private set; }
        public BindListAttribute() { SearchName = null; }
        public BindListAttribute(string searchName) { SearchName = searchName; }
    }

    /// <summary>
    /// Awake()에서 현재 로드된 씬(Hierarchy)의 <b>최상위(Root)</b>에 있는 게임 오브젝트를 찾아 연결합니다.
    /// </summary>
    /// <remarks>
    /// <para><b>[사용 규칙]</b></para>
    /// <list type="number">
    /// <item>찾으려는 오브젝트가 반드시 <b>Hierarchy 최상단</b>에 위치해야 합니다.</item>
    /// <item>오브젝트 이름은 [Bind]와 동일하게 <b>[접두어 + PascalCase 변환 이름]</b>으로 탐색합니다.</item>
    /// <item>멀티 씬(Additive) 환경에서도 로드된 <b>모든 씬의 루트</b>를 검색합니다.</item>
    /// </list>
    /// <para><b>[예시]</b></para>
    /// <code>
    /// [BindRoot] private Camera _mainCamera;
    /// // _mainCamera -> "MainCamera" (PascalCase)
    /// // Camera 타입 -> 접두어 없음 (PrefixTable 참조)
    /// // 최종: Hierarchy 최상단의 "MainCamera" 탐색
    /// 
    /// [BindRoot("MySystem")] private GameObject _sys;
    /// // SearchName 사용 시: Hierarchy 최상단의 "MySystem" 탐색
    /// </code>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class BindRootAttribute : Attribute
    {
        public string SearchName { get; private set; }
        public BindRootAttribute() { SearchName = null; }
        public BindRootAttribute(string searchName) { SearchName = searchName; }
    }

    /// <summary>
    /// Awake()에서 탐색 시, 기본 탐색 범위(자신) 외에 <b>추가로 탐색할 부모 오브젝트</b>를 지정합니다.
    /// </summary>
    /// <remarks>
    /// <para><b>[사용 규칙]</b></para>
    /// <list type="number">
    /// <item>대상 변수는 반드시 <c>Transform</c> 타입이어야 합니다.</item>
    /// <item><c>[SerializeField]</c>를 통해 인스펙터에서 직접 할당해줘야 합니다.</item>
    /// <item>이 어트리뷰트가 붙은 Transform은 <b>탐색 경로(Search Root)</b>에 추가됩니다.</item>
    /// </list>
    /// <para><b>[예시]</b></para>
    /// <code>
    /// [SerializeField] [BindParent] private Transform _abc;
    /// 
    /// [Bind] private Button _closeButton; 
    /// // _closeButton 탐색 범위: 1. 내 자식들(기본) + 2. _abc의 자식들(추가)
    /// </code>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field)]
    public class BindParentAttribute : Attribute { }

    #endregion

    /// <summary>
    /// 이 클래스를 상속받은 클래스 필드가 Component 를 주입할게 있다면<br/>
    /// 종속된 모든 GameObject 들을 탐색하여 적절한 Component 를 주입함.
    /// </summary>
    public abstract class BindBehaviour : MonoBehaviour
    {
        StringBuilder _stringBuilder; // 게임오브젝트를 탐색할 때 사용할 필드

        protected virtual void Awake()
        {
            _stringBuilder = new StringBuilder(30);
            ResolveFields();
        }

        // BindAttribute 가 붙은 필드들을 모두 찾고 의존성을 해결함
        private void ResolveFields()
        {
            Type type = GetType(); // 이 스크립트(를 상속받은 자식 스크립트)의 타입 정보를 가져옴
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance |  // 이 스크립트가 가진
                                                    BindingFlags.NonPublic); // 변수들의 모음
            List<Transform> searchRoots = FindSearchRoots(fieldInfos);       // 탐색범위

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                // 1. [Bind]에 대한 확인
                BindAttribute bindAtt = fieldInfo.GetCustomAttribute<BindAttribute>();
                if (bindAtt != null)
                {
                    string targetName = bindAtt.SearchName ?? fieldInfo.Name;
                    ResolveField(fieldInfo, searchRoots, targetName);
                    continue;
                }

                // 2. [BindList]에 대한 확인
                BindListAttribute bindListAtt = fieldInfo.GetCustomAttribute<BindListAttribute>();
                if (bindListAtt != null)
                {
                    string targetName = bindListAtt.SearchName ?? fieldInfo.Name;
                    ResolveListField(fieldInfo, searchRoots, targetName);
                    continue;
                }

                // 3. [BindRoot]에 대한 확인
                BindRootAttribute bindRootAtt = fieldInfo.GetCustomAttribute<BindRootAttribute>();
                if (bindRootAtt != null)
                {
                    string targetName = bindRootAtt.SearchName ?? fieldInfo.Name;
                    ResolveFieldBySceneRoot(fieldInfo, searchRoots, targetName);
                    continue;
                }
            }
        }

        // BindParentAttribute가 붙은 필드들을 모두 찾고 탐색 범위를 결정
        private List<Transform> FindSearchRoots(FieldInfo[] fieldInfos)
        {
            List<Transform> foundRoots = new List<Transform>();
            foundRoots.Add(this.transform); // 기본 검색 경로는 '나 자신(this.transform)'

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                // 1. [BindParent]에 대한 확인
                BindParentAttribute bindParentAtt =
                    fieldInfo.GetCustomAttribute<BindParentAttribute>();

                if (bindParentAtt == null)
                    continue;

                // 2. 조건 검사
                if (fieldInfo.FieldType != typeof(Transform))
                {
                    Debug.LogWarning($"[BindBehaviour] {fieldInfo.Name}필드는 Transform 타입이 아니므로 무시됩니다.");
                    continue;
                }

                Transform parent = (Transform)fieldInfo.GetValue(this);

                if (parent != null)
                {
                    foundRoots.Add(parent); // 3. 탐색 경로에 추가
                    continue;
                }
                else
                {
                    Debug.LogWarning($"[BindBehaviour] {fieldInfo.Name}필드를 할당하지 않았으므로 무시됩니다.");
                    continue;
                }
            }

            return foundRoots;
        }

        // [Bind] 꼬리표가 붙은 필드의 의존성을 주입
        private void ResolveField(FieldInfo fieldInfo, List<Transform> searchRoots, string fieldName)
        {
            // 1. 탐색할 이름 결정
            Type fieldType = fieldInfo.FieldType; // 연결할 변수의 타입 (예: Button)
            string gameObjectName = ConvertFieldNameToGameObjectName(fieldType, fieldName); // 찾을 이름
            GameObject found = FindGameObjectByName(searchRoots, gameObjectName); // 검색

            // 2. 의존성 주입 시도
            if (!TryBind(fieldInfo, fieldType, found))
                Debug.LogWarning($"[BindBehaviour] 바인딩 실패, '{fieldName}'에 맞는 게임오브젝트의 이름 '{gameObjectName}'을 찾을 수 없습니다.");
        }

        // [BindRoot] 꼬리표가 붙은 필드의 의존성을 주입
        private void ResolveFieldBySceneRoot(FieldInfo fieldInfo, List<Transform> searchRoots, string fieldName)
        {
            // 1. 탐색할 이름 결정
            Type fieldType = fieldInfo.FieldType; // 연결할 변수의 타입
            string gameObjectName = ConvertFieldNameToGameObjectName(fieldType, fieldName); // 찾을 이름

            // 2. 캐시에서 탐색
            if (SceneRootCache.TryGet(gameObjectName, out GameObject found) == false)
            {
                // 실패 시 : 혹시 방금 생성된 녀석일 수 있으니, 캐시를 새로고침
                SceneRootCache.RefreshCache();

                // 다시 확인
                if (SceneRootCache.TryGet(gameObjectName, out found) == false)
                {
                    Debug.LogWarning($"[BindBehaviour] 바인딩 실패, 씬의 최상위 게임오브젝트 중 '{fieldName}'에 맞는 게임오브젝트의 이름 '{gameObjectName}'을 찾을 수 없습니다.");

                    return;
                }
            }
            // 3. 의존성 주입 시도
            if (!TryBind(fieldInfo, fieldType, found))
                Debug.LogWarning($"[BindBehaviour] 바인딩 실패, 씬의 최상위 게임오브젝트 중 '{fieldName}'에 맞는 게임오브젝트의 이름 '{gameObjectName}'을 찾을 수 없습니다.");
        }

        private bool TryBind(FieldInfo fieldInfo, Type fieldType, GameObject foundObject)
        {
            // 1. 게임 오브젝트라면, 바로 주입
            if (fieldType == typeof(GameObject))
            {
                if (foundObject != null)
                {
                    fieldInfo.SetValue(this, foundObject);
                    return true;
                }
            }
            // 2. 컴포넌트라면 찾아서 주입
            else if (typeof(Component).IsAssignableFrom(fieldType))
            {
                if (foundObject != null)
                {
                    Component component = foundObject.GetComponent(fieldType);

                    if (component != null)
                    {
                        fieldInfo.SetValue(this, component);
                        return true;
                    }
                }
            }
            // 3. 여기까지 왔으면 의존성 주입 실패
            return false;
        }

        // [BindList] 꼬리표가 붙은 필드의 의존성을 주입
        private void ResolveListField(FieldInfo fieldInfo, List<Transform> searchRoots, string fieldName)
        {
            // 1. 필드 타입이 배열(Array) 또는 List<T>인지 확인
            Type fieldType = fieldInfo.FieldType; // 예: Button[] 또는 List<Button>
            bool isArray = fieldType.IsArray;
            bool isList = (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>));

            if (isArray == false && isList == false)
            {
                Debug.LogWarning($"[BindBehaviour] 바인딩 실패, '{fieldInfo.Name}' 필드는 배열(Array) 또는 List<T> 타입이 아닙니다.");
                return;
            }

            // 2. 배열 또는 리스트의 '요소 타입'을 가져옴 (예: Button[] -> Button, List<Button> -> Button)
            Type elementType = (isArray == true) ? fieldType.GetElementType()
                                                 : fieldType.GetGenericArguments()[0];

            // 3. '부모 오브젝트'를 찾음
            string parentObjectName = ConvertFieldNameToGameObjectName(elementType, fieldName);
            GameObject parentObject = FindGameObjectByName(searchRoots, parentObjectName);

            if (parentObject == null)
            {
                Debug.LogWarning($"[BindBehaviour] 바인딩 실패, '{parentObjectName}'을(를) 찾을 수 없습니다.");
                return;
            }

            // 4. 실제 자식들을 찾아서 필드에 주입 (헬퍼 메서드 호출)
            BindCollectionChildren(fieldInfo, parentObject, elementType, isArray, fieldName);
        }

        // 실제 자식 오브젝트들을 탐색하여 컬렉션을 생성하고 주입
        private void BindCollectionChildren(FieldInfo fieldInfo, GameObject parent, Type elementType,
                                            bool isArray, string fieldName)
        {
            List<object> foundItems = new List<object>(); // 임시 저장소
            bool isGameObject = (elementType == typeof(GameObject));

            // 1. 부모의 직속 자식들 순회하며 수집
            foreach (Transform child in parent.transform)
            {
                if (isGameObject)
                {
                    foundItems.Add(child.gameObject);
                }
                else
                {
                    Component component = child.GetComponent(elementType);
                    if (component != null)
                    {
                        foundItems.Add(component);
                    }
                    else
                    {
                        Debug.LogWarning($"[BindBehaviour] '{child.name}' 객체에는 '{elementType.Name}' 컴포넌트가 없어 '{fieldName}' 리스트에서 제외됩니다.");
                    }
                }
            }

            // 2. 필드 타입(배열 vs 리스트)에 맞춰 최종 생성 및 주입
            if (isArray)
            {
                // 3-A. 배열 생성 및 값 복사
                Array finalArray = Array.CreateInstance(elementType, foundItems.Count);
                for (int i = 0; i < foundItems.Count; i++)
                {
                    finalArray.SetValue(foundItems[i], i);
                }
                fieldInfo.SetValue(this, finalArray);
            }
            else
            {
                // 3-B. 리스트(List<T>) 생성 및 값 추가
                Type listType = typeof(List<>).MakeGenericType(elementType); // ex. List<Button>
                object finalList = Activator.CreateInstance(listType);       // new List<Button>()
                MethodInfo addMethod = listType.GetMethod("Add");

                foreach (object item in foundItems)
                {
                    addMethod.Invoke(finalList, new object[] { item });
                }
                fieldInfo.SetValue(this, finalList);
            }
        }

        // 필드 이름과 타입으로 패턴을 조합하여 주입할 Component 를 갖는 GameObject 이름으로 변환
        private string ConvertFieldNameToGameObjectName(Type fieldType, string fieldName)
        {
            int start = 0; // 검사할 이름의 시작위치에 대한 변수
            _stringBuilder.Clear();

            // fieldType에 맞는 접두사(prefix)를 가져옴
            string prefix = PrefixTable.GetPrefix(fieldType);

            if (fieldName[0] == '_') // 코드 컨벤션 '_'를 건너뜀
                start = 1;

            else if (fieldName[1] == '_') // 코드 컨벤션 'm_'를 건너뜀
                start = 2;

            if (fieldName.Length < start + 1)
                throw new Exception($"변수명 '{fieldName}'은 너무 짧습니다.");

            // 1. 접두사 붙이기 (예: "Button_")
            _stringBuilder.Append(prefix);
            // 2. 변수 이름의 첫 글자를 대문자로 바꿔서 붙이기 (예: "L")
            _stringBuilder.Append(char.ToUpper(fieldName[start]));
            // 3. 변수 이름의 나머지 글자들 붙이기 (예: "oginButton")
            for (int i = start + 1; i < fieldName.Length; i++)
                _stringBuilder.Append(fieldName[i]);

            return _stringBuilder.ToString();
        }

        // searchRoots를 순회해서 Root(인자) 및 하위 모든 자식 GameObject 탐색 
        private GameObject FindGameObjectByName(List<Transform> searchRoots, string name)
        {
            foreach (Transform root in searchRoots)
            {
                // 1. 혹시 자기 자신인지 확인
                if (root.name == name)
                    return root.gameObject;

                // 2. 바로 아래 '직속 자식' 중에서 찾기
                Transform found = root.Find(name);

                if (found != null)
                    return found.gameObject;

                // 3. '재귀 탐색' 시작.
                GameObject foundRecursively = FindGameObjectRecursively(root, name);

                // 재귀 탐색에서 찾았으면 반환
                if (foundRecursively != null)
                    return foundRecursively;
            }

            return null;
        }

        // 재귀적으로 하위 모든자식 탐색
        private GameObject FindGameObjectRecursively(Transform parent, string name)
        {
            // 1. 부모(parent)의 모든 직속 자식들(child)을 순회
            foreach (Transform child in parent)
            {
                // 1. 이 자식(child)의 이름이 내가 찾는 이름(name)인지 확인
                if (child.name == name)
                    return child.gameObject; // 찾으면 반환

                // 2. 이 자식(child)을 새로운 부모(parent)로 삼아서
                //    'FindGameObjectRecursively' 함수를 다시 호출
                GameObject found = FindGameObjectRecursively(child, name);

                // 3. 손자/증손자... 탐색에서 뭔가를 찾았으면(null이 아니면)
                if (found != null)
                    return found; // 찾은 걸 계속 위로 전달
            }

            return null;
        }

        // 씬 로드 시 루트 오브젝트들을 미리 캐싱해두는 내부 클래스
        private static class SceneRootCache
        {
            private static Dictionary<string, GameObject> _gameObjects;

            static SceneRootCache()
            {
                _gameObjects = new Dictionary<string, GameObject>();

                SceneManager.sceneLoaded += OnSceneLoaded;

                RefreshCache();
            }

            public static bool TryGet(string name, out GameObject go)
            {
                if (_gameObjects == null)
                {
                    go = null;
                    return false;
                }
                return _gameObjects.TryGetValue(name, out go);
            }

            /// <summary> 현재 로드된 모든 씬의 최상위(root)에 있는 오브젝트들을 캐싱합니다. </summary>
            public static void RefreshCache()
            {
                // 1. 리스트 초기화
                if (_gameObjects == null)
                    _gameObjects = new Dictionary<string, GameObject>(32);

                else
                    _gameObjects.Clear();

                // 2. 전체 씬 탐색
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);
                    if (!scene.IsValid()) continue;

                    // 3. 씬의 루트 오브젝트 주입
                    GameObject[] roots = scene.GetRootGameObjects();
                    foreach (GameObject go in roots)
                    {
                        if (!_gameObjects.ContainsKey(go.name))
                            _gameObjects.Add(go.name, go);
                    }
                }
            }

            private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
            {
                RefreshCache();
            }
        }

        // 오브젝트 탐색 로직에서 사용할 접두사(prefix) 테이블 클래스
        private static class PrefixTable
        {
            public static string GetPrefix(Type type)
            {
                return _prefixTable.TryGetValue(type, out string prefix) ? prefix : "";
            }

            private static Dictionary<Type, string> _prefixTable = new Dictionary<Type, string>()
            {
                { typeof(Canvas), "Canvas_" },
                { typeof(Text), "Text (Legacy)_" },
                { typeof(TMP_Text), "Text (TMP)_" },
                { typeof(TextMeshPro), "Text (TMP)_" },
                { typeof(TextMeshProUGUI), "Text (TMP)_" },
                { typeof(TMP_InputField), "InputField (TMP)_" },
                { typeof(Button), "Button_" },
                { typeof(Image), "Image_" },
                { typeof(RawImage), "RawImage_" },
                { typeof(Toggle), "Toggle_" },
                { typeof(Slider), "Slider_" },
                { typeof(TMP_Dropdown), "Dropdown_" },
            };
        }
    }
}