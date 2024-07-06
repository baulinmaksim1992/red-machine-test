using System.Reflection;
using UnityEngine;
using Utils.TransformUtils;

namespace Utils.MonoBehaviourUtils
{
    public class GameMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private ActivityType activityType = ActivityType.Active;
            
            
        protected virtual void OnValidate()
        {
            CheckFields();
            CheckActivity();
        }

        private void CheckFields()
        {
            var type = GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.IsNotSerialized)
                    continue;
                
                if (!field.IsPublic && field.GetCustomAttribute<SerializeField>() == null)
                    continue;
                
                if (field.GetCustomAttribute<NotRequiredField>() != null)
                    continue;
                
                var value = field.GetValue(this);
                if (value == null || value.Equals(null))
                {
                    var hierarchy = transform.GetPath();
                    var message =
                        $"Field <b>{field.Name}</b> not set in component <b>{type.Name}</b> with hierarchy <b>{hierarchy}</b>";
                    
                    Log.Error(this, message);
                }
            }
        }

        private void CheckActivity()
        {
            var isError = false;
            switch (activityType)
            {
                case ActivityType.Any:
                    return;
                    
                case ActivityType.Active:
                    isError = !gameObject.activeSelf;
                    break;
                
                case ActivityType.Inactive:
                    isError = gameObject.activeSelf;
                    break;
            }
            
            if (isError)
                Log.Error(this, $"{name} is supposed to be {activityType}");
        }
    }
}
