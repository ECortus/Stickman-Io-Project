using System;
using SaveableExtension.Runtime;
using StickmanProject.Runtime.SavePrefs;
using UnityEngine;

namespace StickmanIo.Runtime.Player
{
    public delegate void PlayerSerializeAction(ref ProjectSavePrefs savePrefs);
    public delegate void PlayerDeserializeAction(ProjectSavePrefs savePrefs);

    public interface IPlayerSaveable
    {
        event PlayerSerializeAction OnSerialize;
        event PlayerDeserializeAction OnDeserialize;

        void TrySavePrefs();
    }

    public class PlayerSaveableBehaviour : PlayerRigComponent, IPlayerSaveable, ISaveableBehaviour<ProjectSavePrefs>
    {
        const float maxDelayBetweenSaves = 10f;

        PlayerSerializeAction OnSerializeEvent;
        PlayerDeserializeAction OnDeserializeEvent;

        ProjectSavePrefs lastPrefs;

        float lastSaveTime;

        public event PlayerSerializeAction OnSerialize
        {
            add => OnSerializeEvent += value;
            remove => OnSerializeEvent -= value;
        }

        public event PlayerDeserializeAction OnDeserialize
        {
            add
            {
                if (lastPrefs != null)
                {
                    value(lastPrefs);
                }

                OnDeserializeEvent += value;
            }
            remove => OnDeserializeEvent -= value;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            if (!isOwner)
            {
                enabled = false;
                return;
            }

            SaveableSupervisor.AddBehaviour(this);
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            if (!isOwner)
            {
                return;
            }

            SaveableSupervisor.RemoveBehaviour(this);
        }

        public void TrySavePrefs()
        {
            if (!isOwner)
            {
                return;
            }

            if (lastSaveTime + maxDelayBetweenSaves > Time.time)
            {
                return;
            }

            lastSaveTime = Time.time;
            SaveablePrefs.Save<ProjectSavePrefs>();
        }

        public void Serialize(ref ProjectSavePrefs savePrefs)
        {
            OnSerializeEvent?.Invoke(ref savePrefs);
            lastPrefs = savePrefs;
        }

        public void Deserialize(ProjectSavePrefs savePrefs)
        {
            lastPrefs = savePrefs;
            OnDeserializeEvent?.Invoke(savePrefs);
        }
    }
}