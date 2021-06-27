/*
Copyright © 2021 Paitorocxon/ Fabian Müller.
*/



using System;
using System.Collections.Generic;
using System.Text;

namespace Console_Editor {
  class editor {
    public void Editor(string Path) {

      string[] Text = System.IO.File.ReadAllLines(Path);
      int Screen_Width = Console.BufferWidth;
      int Screen_Height = Console.BufferHeight;
      int Cursor_Line = 0;
      int Cursor_Char = 0;
      int Buffer_Height = 0;
      int Buffer_Width = 0;
      int HistoryIndex = 0;
      bool TextChanged = false;
      while (true) {
        Screen_Width = Console.BufferWidth - 1;
        Screen_Height = Console.BufferHeight - 1;
        Console.Clear();

        Console.SetCursorPosition(0, 0);
        for (int i = 0; i <= Screen_Height - 2; i++) {
          Console.CursorVisible = false;
          if (Buffer_Height + i < Text.Length) {
            if (Buffer_Height + i == Cursor_Line) {
              WriteText(Text[Buffer_Height + i], Screen_Width, Screen_Height, Buffer_Width, Buffer_Height);
              Console.WriteLine("");
            } else {
              if (Text[Buffer_Height + i].Length > Screen_Width) {
                Highlighter(Text[Buffer_Height + i].Substring(0, Screen_Width - 2));
              } else {
                Highlighter(Text[Buffer_Height + i]);
              }
              Console.WriteLine("");
            }
          }

        }

        Console.Title = Cursor_Line + "/" + Text.Length + "  " + HistoryIndex + ":";
        try {
          Console.SetCursorPosition((Cursor_Char - Buffer_Width), (Cursor_Line - Buffer_Height));

        } catch (Exception e) {

        }
        Console.CursorVisible = true;

        ConsoleKeyInfo Key = Console.ReadKey();
        if (Key.Key == ConsoleKey.Escape) {
          break;
        } else if (Key.Key == ConsoleKey.LeftArrow) {
          if (Cursor_Char > 0) {
            Cursor_Char--;
          } else if (Cursor_Char < 1 && Cursor_Line > 0) {
            Cursor_Char = Text[(Cursor_Line) - 1].Length;

            if (Text[(Cursor_Line) - 1].Length > Screen_Width - 2) {
              Buffer_Width = (Text[(Cursor_Line) - 1].Length - Screen_Width) + 2;

            }
            Cursor_Line--;
            if (Cursor_Line - Buffer_Height < 1) {
              Buffer_Height--;
            }
          }
          if (Cursor_Char - Buffer_Width < 0) {
            Buffer_Width--;
          }
        } else if (Key.Key == ConsoleKey.RightArrow) {

          if (Cursor_Char - Buffer_Width >= Screen_Width - 1) {
            Buffer_Width++;
          }

          if (Cursor_Char + 1 > Text[(Cursor_Line)].Length) {
            if (Cursor_Line + 1 < Count(Text)) {
              Cursor_Char = 0;
              Cursor_Line++;
              Buffer_Width = 0;
              if (Cursor_Line - Buffer_Height > Screen_Height - 2) {
                Buffer_Height++;
              }
            } else {
              Cursor_Char++;
            }
          } else {
            Cursor_Char++;
          }
        } else if (Key.Key == ConsoleKey.UpArrow) {
          if (Cursor_Line - Buffer_Height > 0) {
            Cursor_Line--;
          }
          if (Cursor_Line - Buffer_Height < 1) {
            Buffer_Height--;
          }
        } else if (Key.Key == ConsoleKey.DownArrow) {
          if (Cursor_Line + 1 <= Count(Text)) {
            Cursor_Line++;
            if (Cursor_Line - Buffer_Height > Screen_Height - 2) {
              Buffer_Height++;
            }
          }
        } else if (Key.Key == ConsoleKey.Backspace) {
          TextChanged = true;

          if (Cursor_Char > 0) {
            Text[Cursor_Line] = Text[Cursor_Line].Remove(Cursor_Char - 1, 1);
            Cursor_Char--;
            if (Text[Cursor_Line].Length < Screen_Width) {
              Buffer_Width = 0;
            }
          } else if (Cursor_Char == 0) {
            if (Text.Length > Cursor_Line && Cursor_Line - 1 >= 0) {
              Text[Cursor_Line - 1] = Text[Cursor_Line - 1] + Text[Cursor_Line];
              Cursor_Char = Text[Cursor_Line - 1].Length - Text[Cursor_Line].Length;

              while (Cursor_Char - Buffer_Width > Screen_Width - 1) {
                Buffer_Width++;
              }
              List < String > tmp = new List < string > (Text);
              tmp.RemoveAt(Cursor_Line);
              Text = tmp.ToArray();
              Cursor_Line--;
            }
          }

        } else if (Key.Key == ConsoleKey.Spacebar) {
          TextChanged = true;
          Text[Cursor_Line] = Text[Cursor_Line].Insert(Cursor_Char, " ");
          Cursor_Char++;
          if (Cursor_Char - Buffer_Width >= Screen_Width - 1) {
            Buffer_Width++;
          }

        } else if (Key.Key == ConsoleKey.Tab) {
          TextChanged = true;

          for (int i = 0; i < 4; i++) {

            Text[Cursor_Line] = Text[Cursor_Line].Insert(Cursor_Char, " ");
            Cursor_Char++;
            if (Cursor_Char - Buffer_Width >= Screen_Width - 1) {
              Buffer_Width++;
            }
          }
        } else if (Key.Key == ConsoleKey.Delete) {
          TextChanged = true;
          if (Cursor_Char + 1 <= Text[Cursor_Line].Length) {
            Text[Cursor_Line] = Text[Cursor_Line].Remove(Cursor_Char, 1);
          } else {
            if (Text.Length >= Cursor_Line + 1 && Text.Length > 1) {
              Text[Cursor_Line] = Text[Cursor_Line] + Text[Cursor_Line + 1];
              List < String > tmp = new List < string > (Text);
              tmp.RemoveAt(Cursor_Line + 1);
              Text = tmp.ToArray();
            }
          }

        } else if (Key.Key == ConsoleKey.K && Key.Modifiers == ConsoleModifiers.Control) {
          TextChanged = true;
          if (Text.Length - 1 >= 1) {
            List < String > tmp = new List < string > (Text);
            tmp.RemoveAt(Cursor_Line);
            Text = tmp.ToArray();
          }

        } else if (Key.Key == ConsoleKey.Enter) {
          TextChanged = true;

          List < String > tmp = new List < string > (Text);
          string T1 = Text[Cursor_Line].Substring(0, Cursor_Char);
          string T2 = Text[Cursor_Line].Substring(Cursor_Char, (Text[Cursor_Line].Length - Cursor_Char));

          tmp[Cursor_Line] = T1;
          tmp.Insert(Cursor_Line + 1, T2);
          Cursor_Line++;
          Cursor_Char = 0;
          Text = tmp.ToArray();
          if (Cursor_Line - Buffer_Height > Screen_Height - 2) {
            Buffer_Height++;
          }

        } else {
          TextChanged = true;
          Text[Cursor_Line] = Text[Cursor_Line].Insert(Cursor_Char, Key.KeyChar.ToString());
          Cursor_Char++;
          if (Cursor_Char - Buffer_Width >= Screen_Width - 1) {
            Buffer_Width++;
          }

        }

        if (Cursor_Line > Text.Length - 1) {
          Cursor_Line = Text.Length - 1;
        }
        if (Buffer_Width < 0) {
          Buffer_Width = 0;
        }
        if (Buffer_Height < 0) {
          Buffer_Height = 0;
        }
        if (Cursor_Char > Text[(Cursor_Line)].Length) {
          Cursor_Char = Text[(Cursor_Line)].Length;
        }
        if (Text[(Cursor_Line)].Length <= Screen_Width) {
          Buffer_Width = 0;
        } else if (Text[Cursor_Line].Length - Buffer_Width < 0) {
          Buffer_Width += Text[Cursor_Line].Length - Screen_Width;
        }

      }

    }

    void WriteText(string Content, int Screen_Width, int Screen_Height, int Buffer_Width, int Buffer_Height) {
      if (Content.Length < Screen_Width) {
        Highlighter(Content);
      } else {
        if (Content.Substring(Buffer_Width).Length > Screen_Width) {
          Highlighter(Content.Substring(Buffer_Width, Screen_Width - 1));
          Console.ForegroundColor = ConsoleColor.White;
          Console.BackgroundColor = ConsoleColor.Blue;
          Console.Write("»");

          Console.BackgroundColor = ConsoleColor.Black;
        } else if (Content.Length <= Screen_Width - 1) {

          Highlighter(Content);
        } else {
          Highlighter(Content.Substring(Buffer_Width));
        }
      }
    }

    void Highlighter(String Text) {
      ConsoleColor tmpf = System.Console.ForegroundColor;
      ConsoleColor tmpb = System.Console.BackgroundColor;
      char[] numbers = {
        '0',
        '1',
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9'
      };
      char[] specialchars1 = {
        ',',
        '.',
        '-',
        ':',
        '_',
        '#',
        '+',
        '*',
        '~',
        '!',
        '§',
        '$',
        '%',
        '&',
        '/',
        '=',
        '?',
        '\\',
        '@'
      };
      char[] specialchars2 = {
        '"',
        '\''
      };
      char[] specialchars3 = {
        '(',
        ')',
        '[',
        ']',
        '{',
        '}'
      };
      char[] specialchars4 = {
        ';',
        '<',
        '>'
      };
      if (Text.Length < 1) {
        System.Console.BackgroundColor = ConsoleColor.DarkRed;
        System.Console.Write(" ");
        System.Console.BackgroundColor = ConsoleColor.Black;
      }
      foreach(char c in Text) {
        bool written = false;
        foreach(char number in numbers) {
          if (c == number) {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.Write(c);
            written = true;
          }
        }
        foreach(char specialchar in specialchars1) {
          if (c == specialchar) {
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.Write(c);
            written = true;
          }
        }
        foreach(char specialchar in specialchars2) {
          if (c == specialchar) {
            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Console.Write(c);
            written = true;
          }
        }
        foreach(char specialchar in specialchars3) {
          if (c == specialchar) {
            System.Console.ForegroundColor = ConsoleColor.Magenta;
            System.Console.Write(c);
            written = true;
          }
        }
        foreach(char specialchar in specialchars4) {
          if (c == specialchar) {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Write(c);
            written = true;
          }
        }
        if (written == false) {
          System.Console.Write(c);
        }
        System.Console.ForegroundColor = tmpf;
        System.Console.BackgroundColor = tmpb;
      }
    }
    int Count(String[] Text) {
      int n = 0;
      for (int i = 0; i < Text.Length; i++) {
        n++;
      }

      return n;
    }

  }

}

public class HistoryEntry {
  public int ID {
    get;
    set;
  }
  public int Buffer_Width {
    get;
    set;
  }
  public int Buffer_Height {
    get;
    set;
  }

  public int Cursor_Char {
    get;
    set;
  }
  public int Cursor_Line {
    get;
    set;
  }
  public string[] CurrentText {
    get;
    set;
  }

  public override string ToString() {
    return Buffer_Width + "/w " + Buffer_Height + "/h" + CurrentText.Length + " Chars";
  }
  public override int GetHashCode() {
    return ID;
  }
}
