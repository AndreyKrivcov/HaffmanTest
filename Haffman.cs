using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TestHaffman
{
    class Haffman<T>
    {
        public KeyValuePair<Dictionary<T, BitArray>, BitArray> Encode(T[] data)
        {
            List<Node<T>> nodes = new List<Node<T>>();
            var map = GetMap(data);
            foreach (var item in map)
            {
                nodes.Add(new Node<T>(item.Key,item.Value));
            }

            while(nodes.Count > 1)
            {
                nodes[0].SetBit(false);
                nodes[1].SetBit(true);
                var item = new Node<T>(nodes[0],nodes[1]);
                nodes.Add(item);
                nodes.RemoveAt(0);
                nodes.RemoveAt(0);
                nodes = nodes.OrderBy(x=>x.Counter).ToList();
            }

            var Key = nodes[0].GetBitKeys();
            BitArray ans = new BitArray(0);
            foreach (var item in data)
            {
                ans.Add(Key[item]);
            }

            return new KeyValuePair<Dictionary<T, BitArray>, BitArray>(Key,ans);
        }
        public T[] Decode(Dictionary<T, BitArray> key, BitArray data)
        {
            List<T> ans = new List<T>();

            BitArray tmp = new BitArray(0);
            foreach (bool item in data)
            { 
                tmp.Length+=1;
                tmp[tmp.Length-1] = item;

                if(key.Values.Any(x=>x.Length == tmp.Length))
                {
                    IEnumerable<KeyValuePair<T,BitArray>> arr = key.Where(x=>x.Value.Length == tmp.Length);
                    foreach (var element in arr)
                    {
                        if((element.Value.Clone() as BitArray).Xor(tmp).OfType<bool>().All(x=>!x))
                        {
                            ans.Add(element.Key);
                            tmp.Length = 0;
                            break;
                        }
                    }
                }
            }

            return ans.ToArray();
        }

        private IOrderedEnumerable<KeyValuePair<T,int>> GetMap(T[] data)
        {
            Dictionary<T,int> map = data.Distinct().ToDictionary(x=>x, x=>0);
            foreach (var item in data)
            {
                map[item]+=1;
            }
            return map.OrderBy(x=>x.Value);
        }
    }

    public static class BitArrayExtention
    {
        public static string GetString(this BitArray array, Encoding encoding)
        {
            int size = (int)Math.Ceiling(array.Length/8.0);
            byte[] data = new byte[size];

            for (int i = 0; i < size; ++i)
            {
                int length = i * 8 + 8;
                
                int m = 1;
                for (int j = i*8; j < length; ++j)
                {
                    if(array.Length <= j)
                    {
                        break;
                    }

                    data[i]+=(byte)(array[j] ? m : 0);
                    m*=2;
                }
            }

            return encoding.GetString(data);
        }
        public static BitArray Add(this BitArray array, BitArray other)
        {
            int length = array.Length;
            array.Length += other.Length;
            foreach (bool item in other)
            {
                array[length++] = item;
            }

            return array;
        }
        public static BitArray Get(this BitArray array, int from, int count)
        {
            BitArray ans = new BitArray(count);
            int length = from + count;
            for (int i = from; i < length; i++)
            {
                ans[i - from] = array[i];
            }

            return ans;
        }
    }
    public static class StringExtention
    {
        public static BitArray GetBitArray(this string s, Encoding encoding, int totalBits)
        {
            BitArray ba = new BitArray(encoding.GetBytes(s));
            ba.Length = totalBits;
            return ba;
        }
    }

    public static class IDictionaryExtention
    {
        public static IDictionary<Key,Value> AddRange<Key,Value>(this IDictionary<Key,Value> dictionary, IEnumerable<KeyValuePair<Key,Value>> other)
        {
            foreach (var item in other)
            {
                dictionary.Add(item.Key,item.Value);
            }

            return dictionary;
        } 
    }
    class Node<T>
    {
        public Node(T value, int counter) : this(null,null)
        {
            Element = value;
            Counter = counter;
        }
        public Node(Node<T> left, Node<T> right)
        {
            Right = right;
            Left = left;

            if(left != null && right != null)
                Counter = left.Counter + right.Counter;
            else if(right == null && left == null)
                IsLeaf = true;
            else
                throw new Exception("Booth incoming nodes must be setted !");
        }

        public int Counter {get;}
        public T Element {get;} = default(T);
        public BitArray Key {get;} = new BitArray(1);

        private Node<T> Left {get;}
        private Node<T> Right {get;}

        public bool IsLeaf {get;} = false;
        private bool setTougle = false;
        public void SetBit(bool bit)
        {
            if(!IsLeaf || !setTougle)
            {
                Key[0] = bit;
                setTougle = true;
            }
            else if(IsLeaf)
            {
                Key.Length+=1;
                Key[Key.Length - 1] = bit;
            }

            if(!IsLeaf)
            {
                Left.SetBit(bit);
                Right.SetBit(bit);
            }
        }
        private BitArray Invert(BitArray arr)
        {
            BitArray data = new BitArray(arr.Length); 
            int length = arr.Length;
            for (int i = 0; i < length; i++)
            {
                data[i] = arr[length-1-i];
            }

            return data;
        }
        public Dictionary<T, BitArray> GetBitKeys()
        {
            Dictionary<T, BitArray> data = new Dictionary<T, BitArray>();
            if(IsLeaf)
            {
                data.Add(Element,Invert(Key));
            }
            else
            {
                data.AddRange(Right.GetBitKeys())
                    .AddRange(Left.GetBitKeys());
            }

            return data;
        }
    }
}