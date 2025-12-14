/*==============================================================================
    C# Coding Convention
        (The 6th New Vegas Conference)
    Finalized Date: 2225-12-12
================================================================================

     본 Coding Convention은 제6차 뉴베가스 회의(The 6th New Vegas Conference
    )에서 협회장 '미스터 하우스(Mr. House)'와 빅 마운틴 연구 개발 센터(Big Mountain Re
    search & Development Center)의 싱크탱크 장 '모비우스 박사(Dr. Mobius)'에 의해 정
    립 되었다.

     이 표준안은 같은 싱크탱크 소속의 저명한 학자 5인(a.k.a. Robode Monkeys)에 의해 철저
    한 감수를 거쳐 승인되었다.

------------------------------------------------------------------------------*/
/*------------------------------------------------------------------------------

    # 00. 코딩하며 생각할 것

    ## 00-A. 코드에도 '단락'을 만들자 (공백 원칙)

    "서로 관련된 코드끼리 묶고, 의미의 전환이 일어날 때 한 줄을 비워준다."

    글을 쓸 때 하나의 주제가 끝나고 다음 주제로 넘어갈 때 문단을 나누는 것과 똑같다.
    코드의 가독성은 코드의 흐름을 얼마나 쉽게 읽을 수 있는지에 따라 결정된다.
    논리적인 코드 덩어리를 하나의 '단락'으로 취급하고, 단락 사이에 공백을 넣어 시각적으로
    구분하면 코드를 훨씬 이해하기 쉬워진다.

    ## 00-B. 좋은 주석의 원칙

    "주석은 '무엇(What)'을 하는지가 아니라, '왜(Why)' 그렇게 했는지를 설명해야 한다."

    코드는 그 자체로 '무엇'을 하는지 이미 설명하고 있다.
    주석은 코드가 설명하지 못하는 배경, 의도, 그리고 이유를 보충하는 역할을 해야 한다.

    나쁜 주석 : `i++;` // i를 1 증가시킨다.
        (코드를 그냥 한글로 번역한 수준)
    좋은 주석 : `i++;` // 다음 아이템을 가리키기 위해 인덱스를 수동으로 조정.
        (왜 i를 증가시켜야만 하는지에 대한 '의도'를 설명)

------------------------------------------------------------------------------*/
/*------------------------------------------------------------------------------

    # 01. 멤버 및 타입 선언 규칙
    
    ## 01-A. 접근 지정자 명시 (IDE0040)
    클래스, 구조체 등의 상위 타입과 필드, 메서드, 프로퍼티 등 모든 멤버의 접근 지정자는
    생략하지 않고 *반드시 명시*합니다. (기본값에 의존하지 않음)
    
    ### Bad :
        class Player          // internal 생략됨
        {
            int hp;           // private 생략됨
            void Attack() { } // private 생략됨
        }
    
    ### Good :
        internal class Player
        {
            private int hp;
            public void Attack() { }
        }
    
    ## 01-B. 타입의 명명 규칙
    모든 타입(클래스, 구조체, 인터페이스, 열거형 등)의 이름은 *파스칼 케이스(PascalCase)*
    로 작성합니다. 각 단어의 첫 글자는 대문자이며, 단어 사이에 밑줄(_)이나 공백이 들어가지
    않습니다.
    메서드, 프로퍼티, 이벤트 등도 동일하게 파스칼 케이스를 사용합니다.
    
    필드의 경우 *카멜 케이스(camelCase)* 를 사용합니다. 첫 글자는 소문자이며, 이후 단어의
    첫 글자는 대문자로 작성하며 공백은 사용하지 않습니다.
    
    프라이빗 필드의 경우, 가독성을 위해 접두사로 밑줄(_)을 붙입니다.
    
    상수는 모두 대문자로 작성하며, 단어 사이에 밑줄(_)을 사용합니다.
    
    ## 01-C. 타입의 작명 규칙
    타입의 이름은 그 역할과 책임을 명확히 드러내는 단어로 구성합니다.
    축약형이나 약어는 피하고, 가능한 한 구체적인 의미를 담도록 합니다.
    예를 들어, `DataManager`보다는 `UserDataManager`가 더 구체적이고 명확한 이름입니다.
    
    이로 인해 코드가 너무 길어진다면, 메서드 내에서 지역 변수로 축약형을 사용할 수 있습니다.
    예를 들어, `UserDataManager` 타입의 인스턴스를 `udm`으로 참조하는 것은 허용됩니다.

------------------------------------------------------------------------------*/
/*------------------------------------------------------------------------------ 

    # 02. Boolean (불리언) 처리 규칙
    
    ## 02-A. 불리언 비교 시 동어반복(Redundancy) 금지
    조건문에서 `true`나 `false`를 명시적으로 비교하지 않습니다.
    불리언 변수 그 자체를 평가합니다.
    
    ### Bad :
        if (isValid == true) return;
    
    ### Good :
        if (isValid) return;
    
    ## 02-B. 긍정형 변수명 사용 (부정의 부정 방지)
    변수명은 항상 *긍정*의 의미로 작성합니다. `isNot...`과 같은 부정형 이름은
    `!isNot...` (부정의 부정) 상황을 만들어 가독성을 해칩니다.
    
    ### Bad :
        bool isNotFound, bool isNotFinished
    
    ### Good :
        bool isFound, bool isFinished
    
    ## 02-C. 부정 연산자(!) 공백 처리
    부정 연산자 사용 시, 가독성을 위해 피연산자와 *한 칸 띄어쓰기*를 합니다.
    
    ### Bad :
        if (!isDead) (붙여 쓰면 묻혀 보일 수 있음)
    
    ### Good :
        if (! isDead)

------------------------------------------------------------------------------*/
/*------------------------------------------------------------------------------ 

    # 03. 초기화 및 호출 규칙
    
    ## 03-A. 배열/리스트 초기화 시 'Single Source of Truth' 준수
    초기화 목록(Initializer list)이 있는 경우, 크기(Length)를 명시적으로 적지 않습니다.
    데이터의 개수(진실의 원천)는 중괄호 {} 안의 내용물이므로, 대괄호 [] 안에 숫자를 적어
    중복 정의하지 않습니다.
    
    ### Bad :
        private int[] ages = new int[5] { 10, 11, 12, 13, 14 };
        5라고 썼는데 실제 데이터가 4개거나 6개면 컴파일 에러 혹은 혼동 유발
    
    ### Good :
        private int[] ages = new int[] { 10, 11, 12, 13, 14 };
        또는
        private int[] ages = { 10, 11, 12, 13, 14 };
    
    ## 03-B. 인자 3개 이상 호출 시 '이름 지정 인수(Named Arguments)' 사용
    함수 호출 시 인자가 3개 이상 넘어갈 경우, 가독성과 실수를 방지하기 위해 매개변수 이름을
    명시합니다.
    
    ### Bad :
        CreateUser("Jane", 25, "Seoul", true); // 각 인자가 무엇을 의미하는지 파악 어려움
    
    ### Good :
        CreateUser(
            name: "Jane",
            age: 25,
            address: "Seoul",
            isActive: true
        );

------------------------------------------------------------------------------*/
/*------------------------------------------------------------------------------ 

    # 04. 타입 추론(var, new)을 통한 보일러플레이트 제거 (IDE0007, IDE0090)
    
    ## 04-A. 타입 추론 활용
    *"같은 타입명이 한 줄에 두 번 등장하는 경우" * *에 한하여, 타입 추론(var 또는 Target
    -typed new)을 사용하여 중복을 제거합니다.
    단, 메서드 이름만으로 타입을 유추할 수 있더라도, 선언부와 할당부의 타입이 명확히 중복될
    때만 적용합니다.
    
    ### Bad (불필요한 반복) :
        Dictionary<string, int> scores = new Dictionary<string, int>();

    ### Good (Target-typed new 사용) :
        Dictionary<string, int> scores = new();

    ### Bad (불명확한 타입) :
        var message = GetString();
    
    ### Good (var 사용) :
        var scores = new Dictionary<string, int>();

------------------------------------------------------------------------------*/
/*------------------------------------------------------------------------------

    # 05. 제어문 블록 규칙 (Control Flow Block Rules)

    if, for, foreach 등의 제어문을 사용할 때, 가독성을 최우선으로 하여 줄바꿈과 블록({})
    사용 규칙을 정의한다.

    ## 05-A. 단일 행의 처리 (Single Line)
    제어문의 본문이 단 한 줄일 경우, 중괄호({})를 생략할 수 있다.
    단, 조건식과 같은 줄에 붙여 쓰지 않고 *반드시 줄을 바꿔 들여쓰기*한다.

    ### Bad (같은 줄 작성) :
    if (! isValid) return;

    ### Good (개행 후 작성) :
    if (! isValid)
        return;

    ## 05-B. 블록의 일관성 (Block Consistency)
    if-else if-else 체인에서, 연결된 조건문 중 *단 하나라도* 중괄호를 사용하는 블록(2줄
    이상)이 있다면, 나머지 모든 블록에도 중괄호를 적용하여 시각적 균형을 맞춘다.

    ### Bad (불균형) :
    if (isDay)
        light.Off();          // 괄호 없음
    else
    {
        light.On();           // 괄호 있음 (시각적 부조화)
        monster.Spawn();
    }

    ### Good (균형 유지) :
    if (isDay)
    {
        light.Off();
    }
    else
    {
        light.On();
        monster.Spawn();
    }

    ## 05-C. 독립된 제어문 간의 공백
    서로 논리적으로 연결되지 않은(else로 묶이지 않은) 독립적인 제어문 사이에는, 한 줄의
    공백을 넣어 구분한다. 이는 문맥의 전환을 시각적으로 알리기 위함이다.

    ### Good :
    if (! isAlive)
        return;

    if (isPoisoned)
    {
        ApplyDamage();
        return;
    }

------------------------------------------------------------------------------*/
/*------------------------------------------------------------------------------

    # 06. 클래스 멤버 작성 순서 (Class Layout)

    클래스 내부의 멤버들은 *가급적 아래 순서(1~6)*를 준수하여 작성합니다. 이는 코드의 탐색
    비용을 줄이고, 중요한 정보(공개 인터페이스)를 상단에 배치하기 위함입니다.

    ## 06-A. 작성 순서 가이드
    1.  프로퍼티 (Properties) : 외부와 소통하는 데이터 접근 통로를 최상단에 둡니다.
    2.  공개 필드 (Public Fields) : *사용을 지양* 하며 프로퍼티로 대체하는 것을 권장
        합니다. 부득이하게 존재할 경우 프로퍼티 바로 아래 배치합니다.
    3.  비공개 필드 (Private Fields) : 내부 상태 저장을 위한 필드입니다.
    4.  생성자 (Constructors) : 객체 생성 및 초기화 로직입니다.
    5.  공개 메서드 (Public Methods) :
            주요 로직 : 가장 자주 사용될 것으로 추정되는 핵심 메서드를 먼저 배치합니다.
            관용적 순서 : `Add` / `Remove`, `Start` / `Stop`와 같이 짝을 이루거나
                         관습적인 순서가 있다면 따릅니다.
    6.  비공개 메서드 (Private Methods) : 공개 메서드의 양에 따라 유연하게 정렬합니다.
            Case A (공개 메서드가 많을 때) : 자주 호출되는(의존성이 높은) 순서대로
                                          정렬하여 중요도를 나타냅니다.
            Case B (공개 메서드가 적을 때) : 코드 흐름상 등장하는(호출되는) 순서대로
                                          정렬하여 읽는 흐름을 끊지 않습니다.
*/
//  ## 06-B. 예시 코드 (Example)
public class Player
{
    // 1. 프로퍼티 (Properties)
    public string Name { get; set; }
    public int Level { get; private set; }

    // 2. 공개 필드 (Public Fields) - 가급적 사용 지양
    public string _legacyTag;

    // 3. 비공개 필드 (Private Fields)
    private int _hp;
    private int _mp;

    // 4. 생성자 (Constructors)
    public Player(string name)
    {
        Name = name;
        _hp = 100;
    }

    // 5. 공개 메서드 (Public Methods)
    // - 핵심 동작이나 관용적 순서(Equip -> UnEquip) 우선
    public void Attack()
    {
        if (CanAttack())
        {
            // ...
        }
    }

    public void Equip(string item) { /* ... */ }
    public void UnEquip(string item) { /* ... */ }

    // 6. 비공개 메서드 (Private Methods)
    // - 공개 메서드가 적으므로, 위 코드에서 호출되는(등장하는) 순서대로 배치
    private bool CanAttack()
    {
        return _hp > 0;
    }

    // ... 기타 비공개 메서드 ...
}

//------------------------------------------------------------------------------



/*==============================================================================
    참고자료 - 리서치 보고서
================================================================================
C# 코딩 컨벤션의 아키텍처적 고찰 및 유니티(Unity) 상속 구조 심층 분석 보고서


서론: 현대적 C# 개발의 이중적 요구사항

소프트웨어 엔지니어링의 역사는 **엄격한 구조적 명시성(Explicitness)**과 구현 논리의 간결
성(Conciseness) 사이의 끊임없는 변증법적 발전 과정으로 해석될 수 있습니다. 특히 마이크로
소프트의.NET 생태계에서 C# 언어는 이러한 두 가지 상반된 가치를 조화시키기 위해 지속적으로
진화해 왔습니다. 초기 C# 1.0이 엄격한 정적 타이핑과 장황한 구문을 강제했다면, 현대의 C#
(9.0~12.0)은 컴파일러의 지능적인 추론 능력을 활용하여 개발자의 인지 부하를 줄이는 데 주력
하고 있습니다.

본 보고서는 귀하가 요청한 두 가지 핵심적인 아키텍처 질문에 대한 심층적인 분석을 제공합니다.
첫째, *명시적 접근 지정자(Explicit Access Modifiers)*의 사용이 왜 현대 C# 표준에서 필
수적인지로 정의되는가에 대한 분석입니다. 둘째, var 키워드와 타겟 타이핑된 new() 표현식을
통한 보일러플레이트(Boilerplate) 코드의 감소가 코드 품질에 미치는 영향입니다. 마지막으로,
귀하가 제시한 유니티(Unity) 스크립트(CreditScreen 및 BindBehaviour)의 Awake 메서드 상
속 문제에서 발생하는 컴파일러 경고의 기술적 의미와 올바른 해결책을 제시합니다.

이 문서는 단순한 코딩 스타일 가이드를 넘어, CLR(Common Language Runtime)의 동작 원리와
유니티 엔진의 생명주기(Lifecycle) 관리 메커니즘을 기반으로 한 전문가 수준의 아키텍처 리포
트입니다.



제1부: 명시적 접근 제어의 철학과 공식 표준

1.1 접근 지정자의 본질과 CLR의 보안 모델
접근 지정자(Access Modifier)는 객체 지향 프로그래밍(OOP)에서 캡슐화(Encapsulation)를
구현하는 가장 기초적이면서도 강력한 도구입니다. C# 컴파일러(Roslyn)가 소스 코드를 중간 언
어(IL: Intermediate Language)로 변환할 때, 이 접근 지정자들은 메타데이터(Metadata)의 
플래그(Flag)로 변환되어 어셈블리 내에 영구적으로 기록됩니다.   

CLR의 JIT(Just-In-Time) 컴파일러는 런타임에 이 메타데이터를 검사하여 메모리 접근의 유효
성을 검증합니다. 즉, private으로 선언된 멤버는 단순히 코드를 읽는 사람에게 "건드리지 말라"
고 경고하는 주석이 아니라, 런타임 환경이 보장하는 강력한 보안 경계입니다. 따라서 접근 지정자
의 올바른 사용은 애플리케이션의 아키텍처적 무결성을 유지하는 첫 번째 방어선입니다.

1.2 암시적 가시성(Implicit Visibility)의 위험성
C# 언어 명세는 접근 지정자가 생략되었을 때의 *기본값(Default)*을 정의하고 있습니다.

클래스(Class), 구조체(Struct): 기본적으로 internal
멤버(필드, 메서드, 프로퍼티): 기본적으로 private    

이러한 기본값은 개발자가 코드를 덜 작성하게 해주지만, 동시에 "의도의 모호성"을 초래합니다.
코드 리뷰어나 유지보수 담당자가 void ProcessData()라는 코드를 보았을 때, 이것이 개발자가
의도적으로 private으로 만든 것인지, 아니면 public으로 만들어야 하는데 실수를 한 것인지 판단
하기 어렵습니다. 이 모호성은 대규모 협업 환경에서 아키텍처의 엔트로피를 증가시키는 주요 원인이
됩니다.

1.3 공식 문서 및 스타일 규칙: IDE0040
귀하가 요청한 *"접근 지정자에 대해 명시적 코딩을 하라"*는 지침은 마이크로소프트의 공식 문서와
.NET 런타임 팀의 코딩 스타일 규칙인 IDE0040에 의해 정의됩니다.

1.3.1 공식 문서 근거
마이크로소프트의 공식 문서 "Access Modifiers (C# Programming Guide)" 및 **"Add
accessibility modifiers (style rule IDE0040)"**은 이 원칙을 명확히 규정하고 있습니다.   

문서 제목: Access Modifiers (C# Programming Guide)

관련 규칙 ID: IDE0040

핵심 내용: 모든 멤버 선언에 대해 접근 지정자를 명시적으로 작성할 것을 권장합니다.
문서에 따르면, 접근 지정자를 명시하는 것은 코드의 가독성을 높이고, 개발자의 의도를 명확히 드러
내는 행위입니다. 특히 private 멤버라 할지라도 private 키워드를 생략하지 않고 명시적으로 작성함
으로써, 해당 멤버가 클래스 내부 구현의 일부임을 시각적으로 확정 짓습니다.

1.3.2 규칙의 적용 및 구성
Visual Studio 및.NET SDK의 분석기(Analyzer)는 .editorconfig 파일을 통해 이 규칙을 강제할
수 있습니다.

옵션 항목	값	설명
Option Name	dotnet_style_require_accessibility_modifiers	접근 지정자 명시 여부 설정
Recommended Value	for_non_interface_members	인터페이스를 제외한 모든 멤버에 명시 (권장)
Alternative Value	always	인터페이스 멤버 포함 항상 명시
권장 설정 예시 (.editorconfig):

Ini, TOML
[*.cs]
dotnet_style_require_accessibility_modifiers = for_non_interface_members:warning
이 설정은 개발자가 void Awake()라고만 작성했을 때 컴파일러 경고를 발생시켜 private void
Awake()로 수정하도록 유도합니다.   

1.3.3 유니티 개발 환경에서의 적용
유니티 개발에서 `` 속성이 붙은 필드의 경우, 접근 지정자의 명시성은 더욱 중요합니다.

C#
// 모호한 표현 (권장하지 않음)
 int speed; 

// 명시적 표현 (권장)
 private int speed;
위의 두 코드는 기능적으로 동일하지만, 두 번째 방식이 유니티의 직렬화 시스템에 노출되면서도
스크립트 상에서는 캡슐화됨을 명확히 보여줍니다. 이는 IDE0040 규칙이 유니티 생태계에서도
유효하고 필수적인 표준임을 시사합니다.



제2부: 보일러플레이트 감소와 구문론적 간결성

2.1 현대 C#의 지향점: 타입 추론과 컨텍스트 인지
"보일러플레이트(Boilerplate)"란 반복적이고 상투적인 코드를 의미하며, 이는 실제 비즈니스
로직의 가시성을 저해합니다. C#은 정적 타이핑(Static Typing)의 안정성을 유지하면서도 동적
언어의 간결함을 수용하기 위해 타입 추론(Type Inference) 기술을 적극적으로 도입했습니다.

귀하가 요청한 *"보일러플레이트를 줄이자는 것 (var 권장이나 new(); 사용 등)"*에 대한 공식
문서는 *"C# Coding Conventions"*와 스타일 규칙 IDE0007, IDE0090에 해당합니다.   

2.2 암시적 타입 선언: var (IDE0007)
var 키워드는 C# 3.0에 도입되었으며, 컴파일러가 변수 할당문의 우항(R-value)을 분석하여
좌항(L-value)의 타입을 결정하는 기능입니다.

2.2.1 공식 문서 근거 및 사용 원칙
마이크로소프트의 "C# Coding Conventions" 문서는 var 사용에 대해 다음과 같은 명확한
가이드라인을 제시합니다.   

원칙 1: 변수의 타입이 우항의 할당 표현식에서 명백할 때(Apparent) var를 사용하십시오.

원칙 2: 타입이 명백하지 않을 때는 var를 사용하지 마십시오.

원칙 3: 내장 숫자 타입(int, string 등)에는 명시적 타입을 선호할 수 있습니다
(팀 스타일에 따라 다름).

올바른 사용 예시:

C#
// 생성자가 타입을 명확히 보여줌
var customer = new Customer(); 

// 팩토리 메서드 이름이 반환 타입을 암시함
var stream = File.Create(path); 
잘못된 사용 예시:

C#
// GetResult()가 무엇을 반환하는지 알 수 없음
var result = GetResult(); 
2.2.2 기술적 심층 분석
var는 런타임 성능에 전혀 영향을 주지 않습니다. 컴파일된 IL 코드에서 var x = 1은
int32 x = 1과 완벽하게 동일합니다. 이는 dynamic 키워드와 구별되어야 하며, var는 강력한 
정적 타이핑을 유지하면서 소스 코드의 시각적 노이즈를 줄이는 도구입니다.   

2.3 타겟 타이핑된 생성자: new() (IDE0090)
C# 9.0에서 도입된 Target-typed new expression은 생성자 호출 시 타입을 생략할 수 있게
해주는 기능입니다. 이는 특히 제네릭(Generic) 타입이 중첩되어 타입 이름이 길어질 때 극적인
가독성 향상을 가져옵니다.   

2.3.1 공식 문서 근거 및 규칙
이 기능은 스타일 규칙 IDE0090: Simplify new expression에 의해 관리됩니다.   

설정 옵션: csharp_style_implicit_object_creation_when_type_is_apparent = true

공식 권장: 생성될 타입이 문맥상 명확할 때 new()를 사용할 것을 권장합니다.

2.3.2 적용 사례 및 효과
필드 초기화나 메서드 인자 전달 시 var를 사용할 수 없는 상황에서 new()는 최고의 효율을 발휘
합니다.

예시 비교:

C#
// 기존 방식 (타입 중복 발생)
private Dictionary<string, List<int>> _cache = new Dictionary<string, List<int>>();

// IDE0090 준수 방식 (타입 중복 제거)
private Dictionary<string, List<int>> _cache = new();
이 방식은 코드 수정 시(예: List를 HashSet으로 변경) 한 곳만 수정하면 되므로 유지보수성을
크게 향상시킵니다.   


제3부: 전체 내용에 대한 요약

3.1 요청 1: 접근 지정자의 명시적 코딩
관련 문서: [Microsoft Learn] Access Modifiers (C# Programming Guide)    

관련 규칙: IDE0040 (Add accessibility modifiers)    

핵심 요약: private이 기본값이라 할지라도 반드시 명시적으로 작성하십시오. 이는 코드의 가독성
을 높이고, 개발자의 의도가 "은닉(Hiding)"임을 명확히 합니다. 유니티의 Awake 예제에서도
private void Awake() 또는 protected override void Awake()와 같이 접근 범위를 명시하는
것이 표준입니다.

3.2 요청 2: 보일러플레이트 감소 (var, new)
관련 문서: [Microsoft Learn] C# Coding Conventions    

관련 규칙: IDE0007 (Use var), IDE0090 (Target-typed new)    

핵심 요약:

var: 우항을 통해 타입이 명확히 드러날 때(예: var sb = new StringBuilder();) 사용하여 로
컬 변수 선언의 중복을 줄이십시오.

new(): 필드 초기화나 인자 전달 시(예: _stringBuilder = new(30);) 사용하여 타입 명시의
중복을 피하십시오. 이는 귀하의 BindBehaviour 코드인 _stringBuilder = new StringBuilder(30);
를 _stringBuilder = new(30);으로 리팩토링할 수 있음을 의미합니다.

이 리포트가 귀하의 아키텍처 결정에 명확한 기준이 되기를 바랍니다. C#의 강력한 타입 시스템과
유니티의 유연성을 조화시키는 것이 고품질 게임 코드의 핵심입니다.


[보고서 끝]



참고 문헌 및 데이터 출처 (Data Sources)

본 보고서의 모든 권고 사항은 마이크로소프트의 공식 문서 및.NET 런타임 저장소의 코딩 가이드
라인에 기반합니다.

: C# 언어 명세 및 접근 지정자 가이드.   

: C# 코딩 컨벤션 및 암시적 타이핑(var) 가이드.   

: 타겟 타이핑된 new 표현식 명세.   

:.NET 코드 스타일 분석기 규칙 (IDE0040, IDE0090).   

:.NET 런타임 아키텍처 및 기여 가이드라인.   

learn.microsoft.com
Access Modifiers (C# Programming Guide) - Microsoft Learn

learn.microsoft.com
Learn the fundamentals of the C# type system

github.com
justinamiller/Coding-Standards: Coding Guidelines for C - GitHub

learn.microsoft.com
IDE0040: Add accessibility modifiers - .NET | Microsoft Learn

learn.microsoft.com
IDE0040: Add accessibility modifiers - .NET | Microsoft Learn

learn.microsoft.com
IDE0040: Add accessibility modifiers - .NET | Microsoft Learn

learn.microsoft.com
IDE0040: Add accessibility modifiers - .NET - Microsoft Learn

learn.microsoft.com
.NET Coding Conventions - C# | Microsoft Learn

learn.microsoft.com
IDE0090: Simplify 'new' expression - .NET | Microsoft Learn

learn.microsoft.com
Implicitly typed local variables (C# Programming Guide) - Microsoft Learn

learn.microsoft.com
new operator - Create and initialize a new instance of a type - C# reference | Microsoft Learn

learn.microsoft.com
Target-typed new expressions - C# feature specifications - Microsoft Learn

learn.microsoft.com
IDE0090: Simplify 'new' expression - .NET | Microsoft Learn

learn.microsoft.com
Access Modifiers - C# reference - Microsoft Learn

learn.microsoft.com
Code-style rules overview - .NET - Microsoft Learn

github.com
runtime/docs/coding-guidelines/coding-style.md at main · dotnet ...
*/