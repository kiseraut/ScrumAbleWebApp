using System.Collections.Generic;
using ScrumAble.Areas.Identity.Data;

namespace ScrumAble.Models
{
    public interface IViewModelTaskAggregate
    {
        ScrumAbleTask Task { get; set; }
        ICollection<ScrumAbleUser> Users { get; set; }
        ICollection<ScrumAbleSprint> Sprints { get; set; }
        ICollection<ScrumAbleStory> Stories { get; set; }
        void Add(ScrumAbleTask scrumAbleTask);
        void Add(ICollection<ScrumAbleUser> scrumAbleUser);
        void Add(ICollection<ScrumAbleSprint> scrumAbleSprint);
        void Add(ICollection<ScrumAbleStory> scrumAbleStory);
    }
}