﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerSuperIO.Base;

namespace ServerSuperIO.Communicate
{
    public class ChannelManager : IChannelManager<string, IChannel>
    {
        private Manager<string, IChannel> _Channels;
        private object _SyncLock;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ChannelManager()
        {
            _SyncLock=new object();
            _Channels = new Manager<string, IChannel>();
        }

        /// <summary>
        /// 同步对象
        /// </summary>
        public object SyncLock
        {
            get { return _SyncLock; }
        }

        /// <summary>
        /// 增加通道
        /// </summary>
        /// <param name="key"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public bool AddChannel(string key, IChannel channel)
        {
            return _Channels.TryAdd(key, channel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IChannel GetChannel(string key)
        {
            if (_Channels.ContainsKey(key))
            {
                IChannel val;
                if (_Channels.TryGetValue(key, out val))
                {
                    return val;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public IChannel GetChannel(string ioPara1, CommunicateType comType)
        {
            IChannel channel = null;
            foreach (KeyValuePair<string,IChannel> c in _Channels)
            {
                if (c.Value.CommunicationType == comType && c.Value.Key==ioPara1)
                {
                    channel = c.Value;
                    break;
                }
            }
            return channel;
        }

        /// <summary>
        /// 获得值
        /// </summary>
        /// <returns></returns>
        public ICollection<IChannel> GetValues()
        {
            return _Channels.Values;
        }

        /// <summary>
        /// 获得关键字
        /// </summary>
        /// <returns></returns>
        public ICollection<string> GetKeys()
        {
            return _Channels.Keys;
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainChannel(string key)
        {
            return _Channels.ContainsKey(key);
        }

        /// <summary>
        /// 删除通道
        /// </summary>
        /// <param name="key"></param>
        public bool RemoveChannel(string key)
        {
            IChannel channel;
            if (_Channels.TryRemove(key, out channel))
            {
                //if (!channel.IsDisposed)
                //{
                //    channel.Close();
                //    channel.Dispose();
                //}
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除所有通道
        /// </summary>
        public void RemoveAllChannel()
        {
            foreach (KeyValuePair<string, IChannel> kv in _Channels)
            {
                if (!kv.Value.IsDisposed)
                {
                    kv.Value.Close();
                    kv.Value.Dispose();
                }
            }
            _Channels.Clear();
        }


        public int ChannelCount
        {
            get { return _Channels.Count; }
        }
    }
}
