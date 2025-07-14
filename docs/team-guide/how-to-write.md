# 書き方.md

## 見出し（Heading）

```markdown
# 見出し1
## 見出し2
### 見出し3
```

## 強調（Emphasis）

```markdown
*斜体* または _斜体_
**太字** または __太字__
~~打ち消し線~~
```

## 箇条書き（List）

```markdown
- リスト1
- リスト2
  - ネスト1
  - ネスト2

1. 番号付き
2. 番号付き
```

## コードブロック（Code）

<インラインコード>

```markdown
これは `インラインコード` です。
```

<コードブロック>

\`\`\`言語名（例: `python`, `csharp`, `bash` など）
コード内容
\`\`\`

```markdown
```csharp
Console.WriteLine("Hello, Markdown!");
```
```

## 引用（Blockquote）

```markdown
> これは引用です。
>> ネストされた引用
```

## リンク（Link）

```markdown
[リンクテキスト](https://example.com)
```

## 画像（Image）

```markdown
![代替テキスト](https://example.com/image.png)
```

## 区切り線（Horizontal Rule）

```markdown
---
```

## 表（Table）

```markdown
| 見出し1 | 見出し2 |
|---------|---------|
| 内容A   | 内容B   |
| 内容C   | 内容D   |
```

## チェックリスト（Task List）

```markdown
- [x] 完了した項目
- [ ] 未完了の項目
```

## HTMLの併用（Raw HTML）

```markdown
<p style="color: red;">HTMLも使えます</p>
```
